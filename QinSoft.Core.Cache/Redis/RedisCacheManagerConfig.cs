using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace QinSoft.Core.Cache.Redis
{
    /// <summary>
    /// Redis缓存管理器配置
    /// </summary>
    [XmlRoot("cacheManager", Namespace = "http://www.qinsoft.com")]
    public class RedisCacheManagerConfig
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
        [XmlElement("cache")]
        [JsonProperty("caches")]
        public RedisCacheItemConfig[] Items { get; set; }

        /// <summary>
        /// 根据指定名称的本地缓存配置项
        /// </summary>
        public RedisCacheItemConfig GetByName(string name)
        {
            return Items?.FirstOrDefault(u => u.Name != null && u.Name.Equals(name));
        }
    }

    /// <summary>
    /// Redis缓存配置项
    /// </summary>
    public class RedisCacheItemConfig
    {
        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 连接池大小
        /// </summary>
        [XmlAttribute("poolSize")]
        [JsonProperty("poolSize")]
        public int PoolSize { get; set; } = 20;

        /// <summary>
        /// Redis连接配置
        /// </summary>
        [XmlElement("configuration")]
        [JsonProperty("configuration")]
        public string Configuration { get; set; }

        /// <summary>
        /// 备份缓存配置
        /// </summary>
        [XmlArray("backups")]
        [XmlArrayItem("backup")]
        [JsonProperty("backups")]
        public BackupRedisCacheItemConfig[] Backups { get; set; }
    }

    /// <summary>
    /// 备份Redis缓存配置项
    /// </summary>
    public class BackupRedisCacheItemConfig
    {
        /// <summary>
        /// Redis连接配置，备份配置
        /// </summary>
        [XmlElement("configuration")]
        [JsonProperty("configuration")]
        public string Configuration { get; set; }
    }
}
