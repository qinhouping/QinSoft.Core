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
        [JsonProperty("kafka")]
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
    }
}
