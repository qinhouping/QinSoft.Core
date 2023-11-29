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
using MQTTnet;
using MQTTnet.Client;
using QinSoft.Core.MQ.Kafka;
using RabbitMQ.Client;
using Google.Protobuf.WellKnownTypes;

namespace QinSoft.Core.MQ.MQTT
{
    public class MQTTManager : IMQTTManager
    {
        /// <summary>
        /// 缓存字典
        /// </summary>
        protected ConcurrentDictionary<string, IMqttClient> CacheDictionary;

        /// <summary>
        /// 日志
        /// </summary>
        protected ILogger logger;

        /// <summary>
        /// Database配置
        /// </summary>
        public MQTTManagerConfig MQTTManagerConfig { get; private set; }

        public MQTTManager(MQTTManagerConfig config) : this(config, NullLoggerFactory.Instance.CreateLogger<MQTTManager>())
        {

        }

        public MQTTManager(MQTTManagerConfig config, ILogger logger)
        {
            ObjectUtils.CheckNull(config, nameof(config));
            ObjectUtils.CheckNull(logger, nameof(logger));
            CacheDictionary = new ConcurrentDictionary<string, IMqttClient>();
            MQTTManagerConfig = config;
            this.logger = logger;
        }

        public MQTTManager(MQTTManagerOptions options, IConfiger configer) : this(options, configer, NullLoggerFactory.Instance)
        {
        }

        public MQTTManager(MQTTManagerOptions options, IConfiger configer, ILoggerFactory loggerFactory)
        {
            ObjectUtils.CheckNull(options, nameof(options));
            ObjectUtils.CheckNull(configer, nameof(configer));
            ObjectUtils.CheckNull(loggerFactory, nameof(loggerFactory));
            CacheDictionary = new ConcurrentDictionary<string, IMqttClient>();
            MQTTManagerConfig = configer.Get<MQTTManagerConfig>(options.ConfigName, options.ConfigFormat);
            logger = loggerFactory.CreateLogger<MQTTManager>();
        }

        public MQTTManager(IOptions<MQTTManagerOptions> optionsAccessor, IConfiger configer) : this(optionsAccessor, configer, NullLoggerFactory.Instance)
        {
        }

        public MQTTManager(IOptions<MQTTManagerOptions> optionsAccessor, IConfiger configer, ILoggerFactory loggerFactory)
        {
            ObjectUtils.CheckNull(optionsAccessor, nameof(optionsAccessor));
            ObjectUtils.CheckNull(configer, nameof(configer));
            ObjectUtils.CheckNull(loggerFactory, nameof(loggerFactory));
            CacheDictionary = new ConcurrentDictionary<string, IMqttClient>();
            MQTTManagerConfig = configer.Get<MQTTManagerConfig>(optionsAccessor.Value.ConfigName, optionsAccessor.Value.ConfigFormat);
            logger = loggerFactory.CreateLogger<MQTTManager>();
        }

        /// <summary>
        /// 获取缓存配置
        /// </summary>
        protected virtual MQTTItemConfig GetMqttItemConfig(string name)
        {
            ObjectUtils.CheckNull(name, nameof(name));
            return MQTTManagerConfig.GetByName(name);
        }

        /// <summary>
        /// 获取默认缓存配置
        /// </summary>
        protected virtual MQTTItemConfig GetDefaultMQTTItemConfig()
        {
            return GetMqttItemConfig(this.MQTTManagerConfig.Primary);
        }

        /// <summary>
        /// 构建mqtt客户端实例
        /// </summary>
        protected virtual IMqttClient BuildClientFromConfig(MQTTItemConfig config)
        {
            var disoptions = new MqttClientDisconnectOptions();
            var options = new MqttClientOptionsBuilder()
             .WithTcpServer(config.Ip, config.Port)
             .WithCredentials(config.Username, config.Password)
             .WithClientId(config.ClientId)
             .WithCleanSession(config.CleanSession)
             .Build();
            //创建连接
            var mqttClient = new MqttFactory().CreateMqttClient();
            mqttClient.ConnectAsync(options).Wait();
            return mqttClient;
        }

        /// <summary>
        /// 获取mqtt客户端
        /// </summary>
        public virtual IMqttClient GetMqtt()
        {
            MQTTItemConfig config = GetDefaultMQTTItemConfig();
            if (config == null)
            {
                throw new MQTTException("not found default mqtt client config");
            }

            IMqttClient client = (IMqttClient)CacheDictionary.GetOrAdd(config.Name, key => BuildClientFromConfig(config));

            logger.LogDebug("get default mqtt client from config");

            return client;
        }

        /// <summary>
        /// 获取mqtt客户端
        /// </summary>
        public virtual async Task<IMqttClient> GetMqttAsync()
        {
            return await ExecuteUtils.ExecuteInTask(GetMqtt);
        }

        /// <summary>
        /// 获取mqtt客户端
        /// </summary>
        public virtual IMqttClient GetMqtt(string name)
        {
            ObjectUtils.CheckNull(name, nameof(name));
            MQTTItemConfig config = GetMqttItemConfig(name);
            if (config == null)
            {
                throw new MQTTException(string.Format("not found mqtt client config:{0}", name));
            }

            IMqttClient client = (IMqttClient)CacheDictionary.GetOrAdd(config.Name, key => BuildClientFromConfig(config));

            logger.LogDebug(string.Format("get mqtt client from config:{0}", name));

            return client;
        }

        /// <summary>
        /// 获取mqtt客户端
        /// </summary>
        public virtual async Task<IMqttClient> GetMqttAsync(string name)
        {
            return await ExecuteUtils.ExecuteInTask(GetMqtt, name);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
            if (CacheDictionary != null)
            {
                foreach (KeyValuePair<string, IMqttClient> pair in CacheDictionary)
                {
                    pair.Value.Dispose();
                }
                CacheDictionary.Clear();
            }
        }
    }
}
