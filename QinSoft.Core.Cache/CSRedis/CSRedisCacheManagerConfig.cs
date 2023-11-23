using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace QinSoft.Core.Cache.CSRedis
{
    /// <summary>
    /// Redis缓存管理器配置
    /// </summary>
    [XmlRoot("csredisManager", Namespace = "http://www.qinsoft.com")]
    public class CSRedisCacheManagerConfig
    {
        /// <summary>
        /// 默认配置
        /// </summary>
        [XmlAttribute("primary")]
        [JsonProperty("primary")]
        public string Primary { get; set; } = "default";

        /// <summary>
        /// 配置项列表
        /// </summary>
        [XmlElement("csredis")]
        [JsonProperty("csredises")]
        public CSRedisCacheItemConfig[] Items { get; set; }

        /// <summary>
        /// 根据指定名称的本地缓存配置项
        /// </summary>
        public CSRedisCacheItemConfig GetByName(string name)
        {
            return Items?.FirstOrDefault(u => u.Name != null && u.Name.Equals(name));
        }
    }

    /// <summary>
    /// Redis缓存配置项
    /// </summary>
    public class CSRedisCacheItemConfig
    {
        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        [XmlElement("connectionString")]
        [JsonProperty("connectionString")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// 哨兵模式
        /// </summary>
        [XmlArray("sentinels")]
        [XmlArrayItem("sentinel")]
        [JsonProperty("sentinels")]
        public string[] Sentinels { get; set; }

        /// <summary>
        /// 集群连接
        /// </summary>
        [XmlArray("connectionStrings")]
        [XmlArrayItem("connectionString")]
        [JsonProperty("connectionStrings")]
        public string[] ConnectionStrings { get; set; }

        /// <summary>
        /// 自定义键规则方法
        /// </summary>
        public string KeyRuleType { get; set; }
    }
}
