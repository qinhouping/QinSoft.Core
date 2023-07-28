using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace QinSoft.Core.Data.MongoDB
{
    /// <summary>
    /// Mongo管理器配置
    /// </summary>
    [XmlRoot("mongodbManager", Namespace = "http://www.qinsoft.com")]
    public class MongoDBManagerConfig
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
        [XmlElement("mongodb")]
        [JsonProperty("mongodbs")]
        public MongoDBItemConfig[] Items { get; set; }

        /// <summary>
        /// 根据指定名称的本地缓存配置项
        /// </summary>
        public MongoDBItemConfig GetByName(string name)
        {
            return Items?.FirstOrDefault(u => u.Name != null && u.Name.Equals(name));
        }
    }

    /// <summary>
    /// 数据库配置项
    /// </summary>
    public class MongoDBItemConfig
    {
        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// mongodb连接配置
        /// </summary>
        [XmlElement("connectionString")]
        [JsonProperty("connectionString")]
        public string ConnectionString { get; set; }


        /// <summary>
        /// mongodb数据库名
        /// </summary>
        [XmlElement("database")]
        [JsonProperty("database")]
        public string Database { get; set; }
    }
}
