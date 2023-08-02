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
        /// 终结点地址和端口
        /// </summary>
        [XmlArray("endPoints")]
        [XmlArrayItem("endPoint")]
        [JsonProperty("endPoints")]
        public string[] EndPoints { get; set; }

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

        /// <summary>
        /// Redis哨兵配置
        /// </summary>
        [XmlElement("sentinel")]
        [JsonProperty("sentinel")]
        public RedisCacheSentinelItemConfig Sentinel { get; set; }
    }

    /// <summary>
    /// Redis缓存哨兵配置项
    /// </summary>
    public class RedisCacheSentinelItemConfig
    {
        /// <summary>
        /// 主节点的名称
        /// </summary>
        [XmlElement("serviceName")]
        [JsonProperty("serviceName")]
        public string ServiceName { get; set; }

        /// <summary>
        /// 哨兵终结点地址和端口
        /// </summary>
        [XmlArray("endPoints")]
        [XmlArrayItem("endPoint")]
        [JsonProperty("endPoints")]
        public string[] EndPoints { get; set; }
    }
}
