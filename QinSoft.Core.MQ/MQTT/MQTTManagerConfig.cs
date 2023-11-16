using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace QinSoft.Core.MQ.MQTT
{
    /// <summary>
    /// Mongo管理器配置
    /// </summary>
    [XmlRoot("mqttManager", Namespace = "http://www.qinsoft.com")]
    public class MQTTManagerConfig
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
        [XmlElement("mqtt")]
        [JsonProperty("mqtts")]
        public MQTTItemConfig[] Items { get; set; }

        /// <summary>
        /// 根据指定名称的本地缓存配置项
        /// </summary>
        public MQTTItemConfig GetByName(string name)
        {
            return Items?.FirstOrDefault(u => u.Name != null && u.Name.Equals(name));
        }
    }

    /// <summary>
    /// 数据库配置项
    /// </summary>
    public class MQTTItemConfig
    {
        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// mqtt连接配置
        /// </summary>
        [XmlElement("ip")]
        [JsonProperty("ip")]
        public string Ip { get; set; }

        /// <summary>
        /// mqtt连接配置
        /// </summary>
        [XmlElement("port")]
        [JsonProperty("port")]
        public int Port { get; set; } = 1883;

        /// <summary>
        /// mqtt用户
        /// </summary>
        [XmlElement("username")]
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// mqtt密码
        /// </summary>
        [XmlElement("password")]
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        /// 客户id
        /// </summary>
        [XmlElement("clientId")]
        [JsonProperty("clientId")]
        public string ClientId { get; set; } = Guid.NewGuid().ToString().Replace("-", "").ToUpper();

        /// <summary>
        /// 客户id
        /// </summary>
        [XmlElement("cleanSession")]
        [JsonProperty("cleanSession")]
        public bool CleanSession { get; set; } = false;
    }
}
