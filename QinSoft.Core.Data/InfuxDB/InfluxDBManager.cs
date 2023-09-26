using QinSoft.Core.Common.Utils;
using QinSoft.Core.Configure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using InfluxDB.Client;
using QinSoft.Core.Data.InfuxDB.Core;

namespace QinSoft.Core.Data.InfluxDB
{
    /// <summary>
    /// Influx管理器
    /// 集成缓存配置，支持多数据源
    /// </summary>
    public class InfluxDBManager : IInfluxDBManager
    {
        /// <summary>
        /// 日志
        /// </summary>
        protected ILogger logger;

        /// <summary>
        /// Influx配置
        /// </summary>
        public InfluxDBManagerConfig InfluxManagerConfig { get; private set; }

        /// <summary>
        /// 缓存字典
        /// </summary>
        protected ConcurrentDictionary<string, IInfluxClient> CacheDictionary;

        public InfluxDBManager(InfluxDBManagerConfig config) : this(config, NullLoggerFactory.Instance.CreateLogger<InfluxDBManager>())
        {

        }

        public InfluxDBManager(InfluxDBManagerConfig config, ILogger logger)
        {
            ObjectUtils.CheckNull(config, "config");
            ObjectUtils.CheckNull(logger, "logger");
            this.CacheDictionary = new ConcurrentDictionary<string, IInfluxClient>();
            InfluxManagerConfig = config;
            this.logger = logger;
        }

        public InfluxDBManager(InfluxDBManagerOptions options, IConfiger configer) : this(options, configer, NullLoggerFactory.Instance)
        {
        }

        public InfluxDBManager(InfluxDBManagerOptions options, IConfiger configer, ILoggerFactory loggerFactory)
        {
            ObjectUtils.CheckNull(options, "options");
            ObjectUtils.CheckNull(configer, "configer");
            ObjectUtils.CheckNull(loggerFactory, "loggerFactory");
            this.CacheDictionary = new ConcurrentDictionary<string, IInfluxClient>();
            InfluxManagerConfig = configer.Get<InfluxDBManagerConfig>(options.ConfigName, options.ConfigFormat);
            logger = loggerFactory.CreateLogger<InfluxDBManager>();
        }

        public InfluxDBManager(IOptions<InfluxDBManagerOptions> optionsAccessor, IConfiger configer) : this(optionsAccessor, configer, NullLoggerFactory.Instance)
        {
        }

        public InfluxDBManager(IOptions<InfluxDBManagerOptions> optionsAccessor, IConfiger configer, ILoggerFactory loggerFactory)
        {
            ObjectUtils.CheckNull(optionsAccessor, "optionsAccessor");
            ObjectUtils.CheckNull(configer, "configer");
            ObjectUtils.CheckNull(loggerFactory, "loggerFactory");
            this.CacheDictionary = new ConcurrentDictionary<string, IInfluxClient>();
            InfluxManagerConfig = configer.Get<InfluxDBManagerConfig>(optionsAccessor.Value.ConfigName, optionsAccessor.Value.ConfigFormat);
            logger = loggerFactory.CreateLogger<InfluxDBManager>();
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        protected virtual InfluxDBItemConfig GetInfluxItemConfig(string name)
        {
            ObjectUtils.CheckNull(name, name);
            return InfluxManagerConfig.GetByName(name);
        }

        /// <summary>
        /// 获取默认配置
        /// </summary>
        protected virtual InfluxDBItemConfig GetDefaultInfluxItemConfig()
        {
            return GetInfluxItemConfig(this.InfluxManagerConfig.Primary);
        }

        /// <summary>
        /// 构建Influx客户端实例
        /// </summary>
        protected virtual IInfluxClient BuildClientFromConfig(InfluxDBItemConfig config)
        {
            ObjectUtils.CheckNull(config, "config");
            InfluxDBClientOptions options = InfluxDBClientOptions.Builder
                .CreateNew()
                .Url(config.Url)
                .AuthenticateToken(config.Token)
                .Org(config.Org)
                .Bucket(config.Bucket)
                .Build();
            return new InfluxClient(options); 
        }

        /// <summary>
        /// 获取Influx客户端
        /// </summary>
        public virtual IInfluxClient GetInflux()
        {
            InfluxDBItemConfig config = GetDefaultInfluxItemConfig();
            if (config == null)
            {
                throw new InfluxDBException("not found default influxdb client config");
            }

            IInfluxClient client = CacheDictionary.GetOrAdd(config.Name, BuildClientFromConfig(config));

            logger.LogDebug("get default influxdb client from config");

            return client;
        }

        /// <summary>
        /// 获取Influx客户端
        /// </summary>
        public virtual async Task<IInfluxClient> GetInfluxAsync()
        {
            return await ExecuteUtils.ExecuteInTask(GetInflux);
        }

        /// <summary>
        /// 获取Influx客户端
        /// </summary>
        public virtual IInfluxClient GetInflux(string name)
        {
            ObjectUtils.CheckNull(name, "name");
            InfluxDBItemConfig config = GetInfluxItemConfig(name);
            if (config == null)
            {
                throw new InfluxDBException(string.Format("not found influxdb client config:{0}", name));
            }

            IInfluxClient client = CacheDictionary.GetOrAdd(config.Name, BuildClientFromConfig(config));

            logger.LogDebug(string.Format("get influxdb client from config:{0}", name));

            return client;
        }

        /// <summary>
        /// 获取Influx客户端
        /// </summary>
        public virtual async Task<IInfluxClient> GetInfluxAsync(string name)
        {
            return await ExecuteUtils.ExecuteInTask(GetInflux, name);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
            if (CacheDictionary != null)
            {
                foreach (KeyValuePair<string, IInfluxClient> pair in CacheDictionary)
                {
                    pair.Value.SafeDispose();
                }
                CacheDictionary.Clear();
            }
        }

    }
}
