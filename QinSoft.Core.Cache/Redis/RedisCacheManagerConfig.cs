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
        public RedisCachePoolItemConfig[] Items { get; set; }

        /// <summary>
        /// 根据指定名称的本地缓存配置项
        /// </summary>
        public RedisCachePoolItemConfig GetByName(string name)
        {
            return Items?.FirstOrDefault(u => u.Name != null && u.Name.Equals(name));
        }
    }

    /// <summary>
    /// Redis缓存配置项
    /// </summary>
    public class RedisCachePoolItemConfig
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
        /// 连接池策略
        /// </summary>
        [XmlAttribute("poolStrategy")]
        [JsonProperty("poolStrategy")]
        public string PoolStrategy { get; set; } = "RoundRobin";

        /// <summary>
        /// Redis连接配置
        /// </summary>
        [XmlElement("main")]
        [JsonProperty("main")]
        public RedisCacheItemConfig Main { get; set; }

        /// <summary>
        /// 备份缓存配置
        /// </summary>
        [XmlArray("backups")]
        [XmlArrayItem("backup")]
        [JsonProperty("backups")]
        public RedisCacheItemConfig[] Backups { get; set; }
    }

    /// <summary>
    /// Redis缓存配置项
    /// </summary>
    public class RedisCacheItemConfig
    {
        /// <summary>
        /// Redis连接配置字符串
        /// </summary>
        [XmlElement("configurationString")]
        [JsonProperty("configurationString")]
        public string ConfigurationString { get; set; }

        /// <summary>
        /// 模式
        /// </summary>
        [XmlElement("commandMap")]
        [JsonProperty("commandMap")]
        public string CommandMap { get; set; } = "Default";

        /// <summary>
        /// 终结点地址和端口
        /// </summary>
        [XmlArray("endPoints")]
        [XmlArrayItem("endPoint")]
        [JsonProperty("endPoints")]
        public string[] EndPoints { get; set; }

        /// <summary>
        /// 主节点的名称
        /// </summary>
        [XmlElement("serviceName")]
        [JsonProperty("serviceName")]
        public string ServiceName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [XmlElement("password")]
        [JsonProperty("password")]
        public string Passowrd { get; set; }

        /// <summary>
        /// 默认数据库
        /// </summary>
        [XmlElement("defaultDatabase")]
        [JsonProperty("defaultDatabase")]
        public int? DefaultDatabase { get; set; }


        /// <summary>
        /// 连接超时，单位ms
        /// </summary>
        [XmlElement("connectTimeout")]
        [JsonProperty("connectTimeout")]
        public int ConnectTimeout { get; set; } = 5000;
    }
}
