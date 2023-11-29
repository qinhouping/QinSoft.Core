using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using QinSoft.Core.Common.Utils;
using QinSoft.Core.Configure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using QinSoft.Core.MQ.RabbitMQ.Core;
using Google.Protobuf.WellKnownTypes;

namespace QinSoft.Core.MQ.RabbitMQ
{
    public class RabbitMQManager : IRabbitMQManager
    {
        /// <summary>
        /// 缓存字典
        /// </summary>
        protected ConcurrentDictionary<string, IRabbitMQClient> CacheDictionary;

        /// <summary>
        /// 日志
        /// </summary>
        protected ILogger logger;

        /// <summary>
        /// Database配置
        /// </summary>
        public RabbitMQManagerConfig RabbitMQManagerConfig { get; private set; }

        public RabbitMQManager(RabbitMQManagerConfig config) : this(config, NullLoggerFactory.Instance.CreateLogger<RabbitMQManager>())
        {

        }

        public RabbitMQManager(RabbitMQManagerConfig config, ILogger logger)
        {
            ObjectUtils.CheckNull(config, nameof(config));
            ObjectUtils.CheckNull(logger, nameof(logger));
            CacheDictionary = new ConcurrentDictionary<string, IRabbitMQClient>();
            RabbitMQManagerConfig = config;
            this.logger = logger;
        }

        public RabbitMQManager(RabbitMQManagerOptions options, IConfiger configer) : this(options, configer, NullLoggerFactory.Instance)
        {
        }

        public RabbitMQManager(RabbitMQManagerOptions options, IConfiger configer, ILoggerFactory loggerFactory)
        {
            ObjectUtils.CheckNull(options, nameof(options));
            ObjectUtils.CheckNull(configer, nameof(configer));
            ObjectUtils.CheckNull(loggerFactory, nameof(loggerFactory));
            CacheDictionary = new ConcurrentDictionary<string, IRabbitMQClient>();
            RabbitMQManagerConfig = configer.Get<RabbitMQManagerConfig>(options.ConfigName, options.ConfigFormat);
            logger = loggerFactory.CreateLogger<RabbitMQManager>();
        }

        public RabbitMQManager(IOptions<RabbitMQManagerOptions> optionsAccessor, IConfiger configer) : this(optionsAccessor, configer, NullLoggerFactory.Instance)
        {
        }

        public RabbitMQManager(IOptions<RabbitMQManagerOptions> optionsAccessor, IConfiger configer, ILoggerFactory loggerFactory)
        {
            ObjectUtils.CheckNull(optionsAccessor, nameof(optionsAccessor));
            ObjectUtils.CheckNull(configer, nameof(configer));
            ObjectUtils.CheckNull(loggerFactory, nameof(loggerFactory));
            CacheDictionary = new ConcurrentDictionary<string, IRabbitMQClient>();
            RabbitMQManagerConfig = configer.Get<RabbitMQManagerConfig>(optionsAccessor.Value.ConfigName, optionsAccessor.Value.ConfigFormat);
            logger = loggerFactory.CreateLogger<RabbitMQManager>();
        }

        /// <summary>
        /// 获取缓存配置
        /// </summary>
        protected virtual RabbitMQItemConfig GetRabbitMQItemConfig(string name)
        {
            ObjectUtils.CheckNull(name, nameof(name));
            return RabbitMQManagerConfig.GetByName(name);
        }

        /// <summary>
        /// 获取默认缓存配置
        /// </summary>
        protected virtual RabbitMQItemConfig GetDefaultRabbitMQItemConfig()
        {
            return GetRabbitMQItemConfig(this.RabbitMQManagerConfig.Primary);
        }

        /// <summary>
        /// 构建mongodb客户端实例
        /// </summary>
        protected virtual IRabbitMQClient BuildClientFromConfig(RabbitMQItemConfig config)
        {
            // 创建连接工厂
            var factory = new ConnectionFactory()
            {
                HostName = config.Hostname,
                Port = config.port,
                VirtualHost = config.VirtualHost,
                UserName = config.Username,
                Password = config.Password
            };
            return new RabbitMQClient(factory);
        }

        /// <summary>
        /// 获取RabbitMQ客户端
        /// </summary>
        public virtual IRabbitMQClient GetRabbitMQ()
        {
            RabbitMQItemConfig config = GetDefaultRabbitMQItemConfig();
            if (config == null)
            {
                throw new RabbitMQException("not found default RabbitMQ client config");
            }

            IRabbitMQClient client = CacheDictionary.GetOrAdd(config.Name, key => BuildClientFromConfig(config));

            logger.LogDebug("get default RabbitMQ client from config");

            return client;
        }

        /// <summary>
        /// 获取RabbitMQ客户端
        /// </summary>
        public virtual async Task<IRabbitMQClient> GetRabbitMQAsync()
        {
            return await ExecuteUtils.ExecuteInTask(GetRabbitMQ);
        }

        /// <summary>
        /// 获取RabbitMQ客户端
        /// </summary>
        public virtual IRabbitMQClient GetRabbitMQ(string name)
        {
            ObjectUtils.CheckNull(name, nameof(name));
            RabbitMQItemConfig config = GetRabbitMQItemConfig(name);
            if (config == null)
            {
                throw new RabbitMQException(string.Format("not found RabbitMQ client config:{0}", name));
            }

            IRabbitMQClient client = CacheDictionary.GetOrAdd(config.Name, key => BuildClientFromConfig(config));

            logger.LogDebug(string.Format("get RabbitMQ client from config:{0}", name));

            return client;
        }

        /// <summary>
        /// 获取RabbitMQ客户端
        /// </summary>
        public virtual async Task<IRabbitMQClient> GetRabbitMQAsync(string name)
        {
            return await ExecuteUtils.ExecuteInTask(GetRabbitMQ, name);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
            if (CacheDictionary != null)
            {
                foreach (KeyValuePair<string, IRabbitMQClient> pair in CacheDictionary)
                {
                    pair.Value.Dispose();
                }
                CacheDictionary.Clear();
            }
        }
    }
}
