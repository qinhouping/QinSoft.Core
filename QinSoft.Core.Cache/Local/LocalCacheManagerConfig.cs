using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace QinSoft.Core.Cache.Local
{
    /// <summary>
    /// 本地缓存配置
    /// </summary>
    [XmlRoot("cacheManager", Namespace = "http://www.qinsoft.com")]
    public class LocalCacheManagerConfig
    {
        /// <summary>
        /// 本地缓存配置项列表
        /// </summary>
        [XmlElement("cache")]
        [JsonProperty("caches")]
        public LocalCacheItemConfig[] Items { get; set; }

        /// <summary>
        /// 根据指定名称的本地缓存配置项
        /// </summary>
        public LocalCacheItemConfig GetByName(string name)
        {
            return Items?.FirstOrDefault(u => u.Name != null && u.Name.Equals(name));
        }
    }

    /// <summary>
    /// 本地缓存配置项
    /// </summary>
    public class LocalCacheItemConfig
    {
        /// <summary>
        /// 名称，默认 "_default"
        /// </summary>
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; } = "_default";

        /// <summary>
        /// 数量限制
        /// </summary>
        [XmlElement("sizeLimie")]
        [JsonProperty("sizeLimie")]
        public int? SizeLimie { get; set; } = null;

        /// <summary>
        /// 压缩比列，默认 "0.05"
        /// </summary>
        [XmlElement("compactionPercentage")]
        [JsonProperty("compactionPercentage")]
        public double CompactionPercentage { get; set; } = 0.05;

        /// <summary>
        /// 移除过期缓存频率，默认 "00:01:00"
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public TimeSpan ExpirationScanFrequency { get; set; } = new TimeSpan(0, 1, 0);

        /// <summary>
        /// 移除过期缓存频率，默认 "00:01:00"
        /// </summary>
        [XmlElement("expirationScanFrequency")]
        [JsonProperty("expirationScanFrequency")]
        public string ExpirationScanFrequencyStr
        {
            get { return ExpirationScanFrequency.ToString(); }
            set { ExpirationScanFrequency = TimeSpan.Parse(value); }
        }
    }
}
