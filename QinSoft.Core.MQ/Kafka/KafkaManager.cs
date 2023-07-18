using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using QinSoft.Core.Common.Utils;
using QinSoft.Core.Configure;
using QinSoft.Core.MQ.Kafka.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QinSoft.Core.MQ.Kafka
{
    public class KafkaManager : IKafkaManager
    {
        /// <summary>
        /// 缓存字典
        /// </summary>
        protected ConcurrentDictionary<string, IKafkaClient<string,string>> CacheDictionary;

        /// <summary>
        /// 日志
        /// </summary>
        protected ILogger logger;

        /// <summary>
        /// Database配置
        /// </summary>
        public KafkaManagerConfig KafkaManagerConfig { get; private set; }

        public KafkaManager(KafkaManagerConfig config) : this(config, NullLoggerFactory.Instance.CreateLogger<KafkaManager>())
        {

        }

        public KafkaManager(KafkaManagerConfig config, ILogger logger)
        {
            ObjectUtils.CheckNull(config, "config");
            ObjectUtils.CheckNull(logger, "logger");
            CacheDictionary = new ConcurrentDictionary<string, IKafkaClient<string,string>>();
            KafkaManagerConfig = config;
            this.logger = logger;
        }

        public KafkaManager(KafkaManagerOptions options, IConfiger configer) : this(options, configer, NullLoggerFactory.Instance)
        {
        }

        public KafkaManager(KafkaManagerOptions options, IConfiger configer, ILoggerFactory loggerFactory)
        {
            ObjectUtils.CheckNull(options, "options");
            ObjectUtils.CheckNull(configer, "configer");
            ObjectUtils.CheckNull(loggerFactory, "loggerFactory");
            CacheDictionary = new ConcurrentDictionary<string, IKafkaClient<string,string>>();
            KafkaManagerConfig = configer.Get<KafkaManagerConfig>(options.ConfigName, options.ConfigFormat);
            logger = loggerFactory.CreateLogger<KafkaManager>();
        }

        public KafkaManager(IOptions<KafkaManagerOptions> optionsAccessor, IConfiger configer) : this(optionsAccessor, configer, NullLoggerFactory.Instance)
        {
        }

        public KafkaManager(IOptions<KafkaManagerOptions> optionsAccessor, IConfiger configer, ILoggerFactory loggerFactory)
        {
            ObjectUtils.CheckNull(optionsAccessor, "optionsAccessor");
            ObjectUtils.CheckNull(configer, "configer");
            ObjectUtils.CheckNull(loggerFactory, "loggerFactory");
            CacheDictionary = new ConcurrentDictionary<string, IKafkaClient<string,string>>();
            KafkaManagerConfig = configer.Get<KafkaManagerConfig>(optionsAccessor.Value.ConfigName, optionsAccessor.Value.ConfigFormat);
            logger = loggerFactory.CreateLogger<KafkaManager>();
        }

        /// <summary>
        /// 获取缓存配置
        /// </summary>
        protected virtual KafkaItemConfig GetKafkaItemConfig(string name)
        {
            ObjectUtils.CheckNull(name, name);
            return KafkaManagerConfig.GetByName(name);
        }

        /// <summary>
        /// 获取默认缓存配置
        /// </summary>
        protected virtual KafkaItemConfig GetDefaultKafkaItemConfig()
        {
            return GetKafkaItemConfig(this.KafkaManagerConfig.Primary);
        }

        /// <summary>
        /// 获取mongodb客户端
        /// </summary>
        public virtual IKafkaClient<string,string> GetKafka()
        {
            KafkaItemConfig config = GetDefaultKafkaItemConfig();
            if (config == null)
            {
                throw new KafkaException("not found default kafka client config");
            }

            IKafkaClient<string,string> client = CacheDictionary.GetOrAdd(config.Name, (key) => new KafkaClient<string,string>(config.BootstrapServers));

            logger.LogDebug("get default kafka client from config");

            return client;
        }

        /// <summary>
        /// 获取mongodb客户端
        /// </summary>
        public virtual async Task<IKafkaClient<string,string>> GetKafkaAsync()
        {
            return await Task.Factory.StartNew(() =>
            {
                return GetKafka();
            });
        }

        /// <summary>
        /// 获取mongodb客户端
        /// </summary>
        public virtual IKafkaClient<string,string> GetKafka(string name)
        {
            ObjectUtils.CheckNull(name, "name");
            KafkaItemConfig config = GetKafkaItemConfig(name);
            if (config == null)
            {
                throw new KafkaException(string.Format("not found kafka client config:{0}", name));
            }

            IKafkaClient<string,string> client = CacheDictionary.GetOrAdd(config.Name, (key) => new KafkaClient<string,string>(config.BootstrapServers));

            logger.LogDebug(string.Format("get kafka client from config:{0}", name));

            return client;
        }

        /// <summary>
        /// 获取mongodb客户端
        /// </summary>
        public virtual async Task<IKafkaClient<string,string>> GetKafkaAsync(string name)
        {
            return await Task.Factory.StartNew(() =>
            {
                return GetKafka(name);
            });
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
            if (CacheDictionary != null)
            {
                foreach (KeyValuePair<string, IKafkaClient<string,string>> pair in CacheDictionary)
                {
                    pair.Value.SafeDispose();
                }
                CacheDictionary.Clear();
            }
        }
    }
}
