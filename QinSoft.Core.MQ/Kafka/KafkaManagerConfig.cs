using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace QinSoft.Core.MQ.Kafka
{
    /// <summary>
    /// Mongo管理器配置
    /// </summary>
    [XmlRoot("kafkaManager", Namespace = "http://www.qinsoft.com")]
    public class KafkaManagerConfig
    {
        /// <summary>
        /// 默认数据库配置
        /// </summary>
        [XmlAttribute("primary")]
        [JsonProperty("primary")]
        public string Primary { get; set; } = "default";

        /// <summary>
        /// 数据库配置列表
        /// </summary>
        [XmlElement("kafka")]
        [JsonProperty("kafkas")]
        public KafkaItemConfig[] Items { get; set; }

        /// <summary>
        /// 根据指定名称的本地缓存配置项
        /// </summary>
        public KafkaItemConfig GetByName(string name)
        {
            return Items?.FirstOrDefault(u => u.Name != null && u.Name.Equals(name));
        }
    }

    /// <summary>
    /// 数据库配置项
    /// </summary>
    public class KafkaItemConfig
    {
        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// kafka连接配置
        /// </summary>
        [XmlElement("bootstrapServers")]
        [JsonProperty("bootstrapServers")]
        public string BootstrapServers { get; set; }

        /// <summary>
        /// kafka连接配置
        /// </summary>
        [XmlElement("sasl")]
        [JsonProperty("sasl")]
        public KafkaItemSaslConfig Sasl { get; set; }

        /// <summary>
        /// 生产者配置
        /// </summary>
        [XmlElement("producer")]
        [JsonProperty("producer")]
        public KafkaItemProducerConfig Producer { get; set; }


        /// <summary>
        /// 生产者配置
        /// </summary>
        [XmlElement("consumer")]
        [JsonProperty("consumer")]
        public KafkaItemConsumerConfig Consumer { get; set; }
    }

    /// <summary>
    /// 生产者配置
    /// </summary>
    public class KafkaItemSaslConfig
    {
        /// <summary>
        /// sasl认证模式
        /// </summary>
        [XmlElement("mechanism")]
        [JsonProperty("mechanism")]
        public string Mechanism { get; set; } = "Plain";

        /// <summary>
        /// sasl用户名
        /// </summary>
        [XmlElement("username")]
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// sasl用户密码
        /// </summary>
        [XmlElement("password")]
        [JsonProperty("password")]
        public string Password { get; set; }
    }


        /// <summary>
        /// 生产者配置
        /// </summary>
    public class KafkaItemProducerConfig
    {
        /// <summary>
        /// 请求的超时时间，单位是毫秒
        /// </summary>
        [XmlElement("requestTimeoutMs")]
        [JsonProperty("requestTimeoutMs")]
        public int RequestTimeoutMs { get; set; } = 30000;

        /// <summary>
        /// 缓冲区等待时间
        /// </summary>
        [XmlElement("lingerMs")]
        [JsonProperty("lingerMs")]
        public int LingerMs { get; set; } = 5;

        /// <summary>
        /// 一个批次中消息的总大小
        /// </summary>
        [XmlElement("batchSize")]
        [JsonProperty("batchSize")]
        public int BatchSize { get; set; } = 1000000;

        /// <summary>
        /// 消息的压缩类型
        /// </summary>
        [XmlElement("compressionType")]
        [JsonProperty("compressionType")]
        public string CompressionType { get; set; } = "None";

        /// <summary>
        /// 消息的压缩级别
        /// </summary>
        [XmlElement("compressionLevel")]
        [JsonProperty("compressionLevel")]
        public int CompressionLevel { get; set; } = -1;

        /// <summary>
        /// 请求的可靠性级别
        /// </summary>
        [XmlElement("acks")]
        [JsonProperty("acks")]
        public string Acks { get; set; } = "Leader";

        /// <summary>
        /// 是否启用幂等性
        /// </summary>
        [XmlElement("enableIdempotence")]
        [JsonProperty("enableIdempotence")]
        public bool EnableIdempotence { get; set; } = false;

        /// <summary>
        /// 键序列化
        /// </summary>
        [XmlElement("keySerializer")]
        [JsonProperty("keySerializer")]
        public string KeySerializer { get; set; } = null;

        /// <summary>
        /// 键序列化
        /// </summary>
        [XmlElement("valueSerializer")]
        [JsonProperty("valueSerializer")]
        public string ValueSerializer { get; set; } = null;
    }

    /// <summary>
    /// 消费者配置
    /// </summary>
    public class KafkaItemConsumerConfig
    {
        /// <summary>
        /// 消费组ID
        /// </summary>
        [XmlElement("groupId")]
        [JsonProperty("groupId")]
        public string GroupId { get; set; }

        /// <summary>
        /// 初始偏移量的策略
        /// </summary>
        [XmlElement("autoOffsetReset")]
        [JsonProperty("autoOffsetReset")]
        public string AutoOffsetReset { get; set; } = "Latest";

        /// <summary>
        /// 是否自动提交
        /// </summary>
        [XmlElement("enableAutoCommit")]
        [JsonProperty("enableAutoCommit")]
        public bool EnableAutoCommit { get; set; } = true;

        /// <summary>
        /// 自动提交的时间间隔（以毫秒为单位）
        /// </summary>
        [XmlElement("autoCommitIntervalMs")]
        [JsonProperty("autoCommitIntervalMs")]
        public int AutoCommitIntervalMs { get; set; } = 5000;

        /// <summary>
        /// 在拉取消息之间允许的最大间隔时间（以毫秒为单位）
        /// </summary>
        [XmlElement("MaxPollIntervalMs")]
        [JsonProperty("MaxPollIntervalMs")]
        public int MaxPollIntervalMs { get; set; } = 300000;

        /// <summary>
        /// 分区的分配策略
        /// </summary>
        [XmlElement("partitionAssignmentStrategy")]
        [JsonProperty("partitionAssignmentStrategy")]
        public string PartitionAssignmentStrategy { get; set; } = "Range";

        /// <summary>
        /// 键反序列化
        /// </summary>
        [XmlElement("KeyDeserializer")]
        [JsonProperty("KeyDeserializer")]
        public string KeyDeserializer { get; set; } = null;

        /// <summary>
        /// 键反序列化
        /// </summary>
        [XmlElement("valueDeserializer")]
        [JsonProperty("valueDeserializer")]
        public string ValueDeserializer { get; set; } = null;
    }
}
