using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace QinSoft.Core.Data.Elasticsearch
{
    /// <summary>
    /// elasticsearch管理器配置
    /// </summary>
    [XmlRoot("elasticsearchManager", Namespace = "http://www.qinsoft.com")]
    public class ElasticsearchManagerConfig
    {
        /// <summary>
        /// 默认配置
        /// </summary>
        [XmlAttribute("primary")]
        [JsonProperty("primary")]
        public string Primary { get; set; } = "default";

        /// <summary>
        /// elasticsearch配置列表
        /// </summary>
        [XmlElement("elasticsearch")]
        [JsonProperty("elasticsearch")]
        public ElasticsearchItemConfig[] Items { get; set; }

        /// <summary>
        /// 根据指定名称的本地缓存配置项
        /// </summary>
        public ElasticsearchItemConfig GetByName(string name)
        {
            return Items?.FirstOrDefault(u => u.Name != null && u.Name.Equals(name));
        }
    }

    /// <summary>
    /// 数据库配置项
    /// </summary>
    public class ElasticsearchItemConfig
    {
        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("username")]
        [JsonProperty("username")]
        public string Username { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("password")]
        [JsonProperty("password")]
        public string Password { get; set; }

        /// <summary>
        /// 认证令牌
        /// </summary>
        [XmlAttribute("apiKey")]
        [JsonProperty("apiKey")]
        public string ApiKey { get; set; }

        /// <summary>
        /// elasticsearch默认索引
        /// </summary>
        [XmlAttribute("indexName")]
        [JsonProperty("indexName")]
        public string DefaultIndexName { get; set; }

        /// <summary>
        /// elasticsearch地址
        /// </summary>
        [XmlElement("url")]
        [JsonProperty("url")]
        public string[] Urls { get; set; }
    }
}
