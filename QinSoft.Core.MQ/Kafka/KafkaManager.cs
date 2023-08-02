using Confluent.Kafka;
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
        protected ConcurrentDictionary<string, IKafkaClient> CacheDictionary;

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
            CacheDictionary = new ConcurrentDictionary<string, IKafkaClient>();
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
            CacheDictionary = new ConcurrentDictionary<string, IKafkaClient>();
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
            CacheDictionary = new ConcurrentDictionary<string, IKafkaClient>();
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
        /// 构建mongodb客户端实例
        /// </summary>
        protected virtual IKafkaClient<TKEY, TVALUE> BuildClientFromConfig<TKEY, TVALUE>(KafkaItemConfig config)
        {
            ObjectUtils.CheckNull(config, "config");
            ProducerConfig producerConfig = new ProducerConfig()
            {
                BootstrapServers = config.BootstrapServers,
                RequestTimeoutMs = config.Producer?.RequestTimeoutMs,
                LingerMs = config.Producer?.LingerMs,
                CompressionType = config.Producer?.CompressionType.ParseEnum<CompressionType>(),
                CompressionLevel = config.Producer?.CompressionLevel,
                Acks = config.Producer?.Acks.ParseEnum<Acks>(),
                EnableIdempotence = config.Producer?.EnableIdempotence,
                BatchSize = config.Producer?.BatchSize
            };
            ISerializer<TKEY> keySerializer = config.Producer?.KeySerializer.IsEmpty() == true ? null : (ISerializer<TKEY>)Activator.CreateInstance(Type.GetType(config.Producer?.KeySerializer));
            ISerializer<TVALUE> valueSerializer = config.Producer?.ValueSerializer.IsEmpty() == true ? null : (ISerializer<TVALUE>)Activator.CreateInstance(Type.GetType(config.Producer?.ValueSerializer));

            ConsumerConfig consumerConfig = new ConsumerConfig()
            {
                BootstrapServers = config.BootstrapServers,
                GroupId = config.Consumer?.GroupId,
                AutoOffsetReset = config.Consumer?.AutoOffsetReset.ParseEnum<AutoOffsetReset>(),
                EnableAutoCommit = config.Consumer?.EnableAutoCommit,
                AutoCommitIntervalMs = config.Consumer?.AutoCommitIntervalMs,
                MaxPollIntervalMs = config.Consumer?.MaxPollIntervalMs,
                PartitionAssignmentStrategy = config.Consumer?.PartitionAssignmentStrategy.ParseEnum<PartitionAssignmentStrategy>()
            };
            IDeserializer<TKEY> keyDeserializer = config.Consumer?.KeyDeserializer.IsEmpty() == true ? null : (IDeserializer<TKEY>)Activator.CreateInstance(Type.GetType(config.Consumer?.KeyDeserializer));
            IDeserializer<TVALUE> valueDeserializer = config.Consumer?.ValueDeserializer.IsEmpty() == true ? null : (IDeserializer<TVALUE>)Activator.CreateInstance(Type.GetType(config.Consumer?.ValueDeserializer));

            return new KafkaClient<TKEY, TVALUE>(producerConfig, keySerializer, valueSerializer, consumerConfig, keyDeserializer, valueDeserializer);
        }

        /// <summary>
        /// 获取kafka客户端
        /// </summary>
        public virtual IKafkaClient<TKEY, TVALUE> GetKafka<TKEY, TVALUE>()
        {
            KafkaItemConfig config = GetDefaultKafkaItemConfig();
            if (config == null)
            {
                throw new Core.KafkaException("not found default kafka client config");
            }

            IKafkaClient<TKEY, TVALUE> client = (IKafkaClient<TKEY, TVALUE>)CacheDictionary.GetOrAdd(config.Name, key => BuildClientFromConfig<TKEY, TVALUE>(config));

            logger.LogDebug("get default kafka client from config");

            return client;
        }

        /// <summary>
        /// 获取kafka客户端
        /// </summary>
        public virtual async Task<IKafkaClient<TKEY, TVALUE>> GetKafkaAsync<TKEY, TVALUE>()
        {
            return await ExecuteUtils.ExecuteInTask(GetKafka<TKEY, TVALUE>);
        }

        /// <summary>
        /// 获取kafka客户端
        /// </summary>
        public virtual IKafkaClient<TKEY, TVALUE> GetKafka<TKEY, TVALUE>(string name)
        {
            ObjectUtils.CheckNull(name, "name");
            KafkaItemConfig config = GetKafkaItemConfig(name);
            if (config == null)
            {
                throw new Core.KafkaException(string.Format("not found kafka client config:{0}", name));
            }

            IKafkaClient<TKEY, TVALUE> client = (IKafkaClient<TKEY, TVALUE>)CacheDictionary.GetOrAdd(config.Name, key => BuildClientFromConfig<TKEY, TVALUE>(config));

            logger.LogDebug(string.Format("get kafka client from config:{0}", name));

            return client;
        }

        /// <summary>
        /// 获取kafka客户端
        /// </summary>
        public virtual async Task<IKafkaClient<TKEY, TVALUE>> GetKafkaAsync<TKEY, TVALUE>(string name)
        {
            return await ExecuteUtils.ExecuteInTask(GetKafka<TKEY, TVALUE>, name);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
            if (CacheDictionary != null)
            {
                foreach (KeyValuePair<string, IKafkaClient> pair in CacheDictionary)
                {
                    pair.Value.SafeDispose();
                }
                CacheDictionary.Clear();
            }
        }
    }
}
