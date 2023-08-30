using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace QinSoft.Core.Data.Solr
{
    /// <summary>
    /// Solr管理器配置
    /// </summary>
    [XmlRoot("solrManager", Namespace = "http://www.qinsoft.com")]
    public class SolrManagerConfig
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
        [XmlElement("solr")]
        [JsonProperty("solrs")]
        public SolrItemConfig[] Items { get; set; }

        /// <summary>
        /// 根据指定名称的Solr配置项
        /// </summary>
        public SolrItemConfig GetByName(string name)
        {
            return Items?.FirstOrDefault(u => u.Name != null && u.Name.Equals(name));
        }
    }

    /// <summary>
    /// Solr配置项
    /// </summary>
    public class SolrItemConfig
    {
        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// solr地址
        /// </summary>
        [XmlElement("url")]
        [JsonProperty("url")]
        public string Url { get; set; }

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
    }
}
