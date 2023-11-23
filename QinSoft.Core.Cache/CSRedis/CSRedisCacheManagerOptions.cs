using QinSoft.Core.Configure;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Cache.CSRedis
{
    /// <summary>
    /// Redis缓存管理器配置选项
    /// </summary>
    public class CSRedisCacheManagerOptions
    {
        /// <summary>
        /// 配置名称，默认 "CSRedisCacheManagerConfig"
        /// </summary>
        public string ConfigName { get; set; } = "CSRedisCacheManagerConfig";

        /// <summary>
        /// 配置格式，默认 "ConfigFormat.XML"
        /// </summary>
        public ConfigFormat ConfigFormat { get; set; } = ConfigFormat.XML;
    }
}
