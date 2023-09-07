using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace QinSoft.Core.MQ.RabbitMQ
{
    /// <summary>
    /// RabbitMQ管理器配置
    /// </summary>
    [XmlRoot("rabbitMQManager", Namespace = "http://www.qinsoft.com")]
    public class RabbitMQManagerConfig
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
        [XmlElement("rabbitMQ")]
        [JsonProperty("rabbitMQ")]
        public RabbitMQItemConfig[] Items { get; set; }

        /// <summary>
        /// 根据指定名称的本地缓存配置项
        /// </summary>
        public RabbitMQItemConfig GetByName(string name)
        {
            return Items?.FirstOrDefault(u => u.Name != null && u.Name.Equals(name));
        }
    }

    /// <summary>
    /// 数据库配置项
    /// </summary>
    public class RabbitMQItemConfig
    {
        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 主机名
        /// </summary>
        [XmlElement("hostname")]
        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        /// <summary>
        /// 主机端口
        /// </summary>
        [XmlElement("port")]
        [JsonProperty("port")]
        public int port { get; set; } = 5672;

        /// <summary>
        /// 虚拟主机
        /// </summary>
        [XmlElement("virtualHost")]
        [JsonProperty("virtualHost")]
        public string VirtualHost { get; set; } = "/";

        /// <summary>
        /// 用户名
        /// </summary>
        [XmlElement("username")]
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// 用户密码
        /// </summary>
        [XmlElement("password")]
        [JsonProperty("password")]
        public string Password { get; set; }
    }
}
