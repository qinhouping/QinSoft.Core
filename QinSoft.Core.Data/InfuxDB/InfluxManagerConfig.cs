using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace QinSoft.Core.Data.Influx
{
    /// <summary>
    /// Influx管理器配置
    /// </summary>
    [XmlRoot("influxManager", Namespace = "http://www.qinsoft.com")]
    public class InfluxManagerConfig
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
        [XmlElement("influx")]
        [JsonProperty("influxs")]
        public InfluxItemConfig[] Items { get; set; }

        /// <summary>
        /// 根据指定名称的Influx配置项
        /// </summary>
        public InfluxItemConfig GetByName(string name)
        {
            return Items?.FirstOrDefault(u => u.Name != null && u.Name.Equals(name));
        }
    }

    /// <summary>
    /// Influx配置项
    /// </summary>
    public class InfluxItemConfig
    {
        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Influx连接配置
        /// </summary>
        [XmlElement("connectionString")]
        [JsonProperty("connectionString")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Influx超时
        /// </summary>
        [XmlElement("sessionTimeout")]
        [JsonProperty("sessionTimeout")]
        public int SessionTimeout { get; set; } = 10000;

        /// <summary>
        /// Influx连接配置
        /// </summary>
        [XmlElement("authInfo")]
        [JsonProperty("authInfos")]
        public InfluxAuthConfig[] AuthInfos { get; set; }
    }

    /// <summary>
    /// Influx认证配置
    /// </summary>
    public class InfluxAuthConfig
    {
        [XmlElement("schema")]
        [JsonProperty("schema")]
        public string Schema { get; set; }

        [XmlElement("auth")]
        [JsonProperty("auth")]
        public string Auth { get; set; }
    }
}
