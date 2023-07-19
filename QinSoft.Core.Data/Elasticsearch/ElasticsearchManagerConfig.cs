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
        [XmlElement("mongodb")]
        [JsonProperty("mongodb")]
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
        /// elasticsearch地址
        /// </summary>
        [XmlElement("url")]
        [JsonProperty("url")]
        public string[] Urls { get; set; }
    }
}
