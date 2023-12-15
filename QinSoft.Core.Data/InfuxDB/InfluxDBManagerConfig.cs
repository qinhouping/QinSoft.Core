using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace QinSoft.Core.Data.InfluxDB
{
    /// <summary>
    /// Influx管理器配置
    /// </summary>
    [XmlRoot("influxdbManager", Namespace = "http://www.qinsoft.com")]
    public class InfluxDBManagerConfig
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
        [XmlElement("influxdb")]
        [JsonProperty("influxdbs")]
        public InfluxDBItemConfig[] Items { get; set; }

        /// <summary>
        /// 根据指定名称的Influx配置项
        /// </summary>
        public InfluxDBItemConfig GetByName(string name)
        {
            return Items?.FirstOrDefault(u => u.Name != null && u.Name.Equals(name));
        }
    }

    /// <summary>
    /// Influx配置项
    /// </summary>
    public class InfluxDBItemConfig
    {
        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Influx连接地址
        /// </summary>
        [XmlElement("url")]
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Influx连接token
        /// </summary>
        [XmlElement("token")]
        [JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        /// Influx默认组织
        /// </summary>
        [XmlElement("org")]
        [JsonProperty("org")]
        public string Org { get; set; } = "-";

        /// <summary>
        /// Influx默认桶
        /// </summary>
        [XmlElement("bucket")]
        [JsonProperty("bucket")]
        public string Bucket { get; set; }

        /// <summary>
        /// Influx超时时间 单位秒
        /// </summary>
        [XmlElement("timeout")]
        [JsonProperty("timeout")]
        public int Timeout { get; set; } = 10;
    }
}
