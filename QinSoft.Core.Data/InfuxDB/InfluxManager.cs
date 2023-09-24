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
using org.apache.Influx;
using QinSoft.Core.Data.Influx.Core;

namespace QinSoft.Core.Data.Influx
{
    /// <summary>
    /// Influx管理器
    /// 集成缓存配置，支持多数据源
    /// </summary>
    public class InfluxManager : IInfluxManager
    {
        /// <summary>
        /// 日志
        /// </summary>
        protected ILogger logger;

        /// <summary>
        /// Influx配置
        /// </summary>
        public InfluxManagerConfig InfluxManagerConfig { get; private set; }

        /// <summary>
        /// 缓存字典
        /// </summary>
        protected ConcurrentDictionary<string, IInflux> CacheDictionary;

        public InfluxManager(InfluxManagerConfig config) : this(config, NullLoggerFactory.Instance.CreateLogger<InfluxManager>())
        {

        }

        public InfluxManager(InfluxManagerConfig config, ILogger logger)
        {
            ObjectUtils.CheckNull(config, "config");
            ObjectUtils.CheckNull(logger, "logger");
            this.CacheDictionary = new ConcurrentDictionary<string, IInflux>();
            InfluxManagerConfig = config;
            this.logger = logger;
        }

        public InfluxManager(InfluxManagerOptions options, IConfiger configer) : this(options, configer, NullLoggerFactory.Instance)
        {
        }

        public InfluxManager(InfluxManagerOptions options, IConfiger configer, ILoggerFactory loggerFactory)
        {
            ObjectUtils.CheckNull(options, "options");
            ObjectUtils.CheckNull(configer, "configer");
            ObjectUtils.CheckNull(loggerFactory, "loggerFactory");
            this.CacheDictionary = new ConcurrentDictionary<string, IInflux>();
            InfluxManagerConfig = configer.Get<InfluxManagerConfig>(options.ConfigName, options.ConfigFormat);
            logger = loggerFactory.CreateLogger<InfluxManager>();
        }

        public InfluxManager(IOptions<InfluxManagerOptions> optionsAccessor, IConfiger configer) : this(optionsAccessor, configer, NullLoggerFactory.Instance)
        {
        }

        public InfluxManager(IOptions<InfluxManagerOptions> optionsAccessor, IConfiger configer, ILoggerFactory loggerFactory)
        {
            ObjectUtils.CheckNull(optionsAccessor, "optionsAccessor");
            ObjectUtils.CheckNull(configer, "configer");
            ObjectUtils.CheckNull(loggerFactory, "loggerFactory");
            this.CacheDictionary = new ConcurrentDictionary<string, IInflux>();
            InfluxManagerConfig = configer.Get<InfluxManagerConfig>(optionsAccessor.Value.ConfigName, optionsAccessor.Value.ConfigFormat);
            logger = loggerFactory.CreateLogger<InfluxManager>();
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        protected virtual InfluxItemConfig GetInfluxItemConfig(string name)
        {
            ObjectUtils.CheckNull(name, name);
            return InfluxManagerConfig.GetByName(name);
        }

        /// <summary>
        /// 获取默认配置
        /// </summary>
        protected virtual InfluxItemConfig GetDefaultInfluxItemConfig()
        {
            return GetInfluxItemConfig(this.InfluxManagerConfig.Primary);
        }

        /// <summary>
        /// 资源释放事件处理
        /// </summary>
        protected virtual void InfluxDisposeHandle(object sender,EventArgs args)
        {
            IInflux Influx = (IInflux)sender;
            if (CacheDictionary != null)
            {
                CacheDictionary.Remove(Influx.ConfigId,out _);
            }
            Influx.Disposed -= InfluxDisposeHandle;
        }

        /// <summary>
        /// 构建Influx客户端实例
        /// </summary>
        protected virtual IInflux BuildClientFromConfig(InfluxItemConfig config)
        {
            ObjectUtils.CheckNull(config, "config");
            IInflux Influx = new Core.Influx(config.ConnectionString, config.SessionTimeout)
            {
                ConfigId = config.Name
            };
            if(!config.AuthInfos.IsEmpty()){
               foreach(InfluxAuthConfig authConfig in config.AuthInfos )
               {
                    Influx.AddAuthInfo(authConfig.Schema, authConfig.Auth);
               }
            }
            Influx.Disposed += InfluxDisposeHandle;
            return Influx;
        }

        /// <summary>
        /// 获取Influx客户端
        /// </summary>
        public virtual IInflux GetInflux()
        {
            InfluxItemConfig config = GetDefaultInfluxItemConfig();
            if (config == null)
            {
                throw new InfluxException("not found default Influx client config");
            }

            IInflux client = CacheDictionary.GetOrAdd(config.Name, key => BuildClientFromConfig(config));

            logger.LogDebug("get default Influx client from config");

            return client;
        }

        /// <summary>
        /// 获取Influx客户端
        /// </summary>
        public virtual async Task<IInflux> GetInfluxAsync()
        {
            return await ExecuteUtils.ExecuteInTask(GetInflux);
        }

        /// <summary>
        /// 获取Influx客户端
        /// </summary>
        public virtual IInflux GetInflux(string name)
        {
            ObjectUtils.CheckNull(name, "name");
            InfluxItemConfig config = GetInfluxItemConfig(name);
            if (config == null)
            {
                throw new InfluxException(string.Format("not found Influx client config:{0}", name));
            }

            IInflux client =CacheDictionary.GetOrAdd(config.Name, key=> BuildClientFromConfig(config));

            logger.LogDebug(string.Format("get Influx client from config:{0}", name));

            return client;
        }

        /// <summary>
        /// 获取Influx客户端
        /// </summary>
        public virtual async Task<IInflux> GetInfluxAsync(string name)
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
                foreach (KeyValuePair<string, IInflux> pair in CacheDictionary)
                {
                    pair.Value.SafeDispose();
                }
                CacheDictionary.Clear();
            }
        }

    }
}
