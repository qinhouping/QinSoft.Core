using QinSoft.Core.Configure;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Cache.Local
{
    /// <summary>
    /// 本地缓存管理器配置选项
    /// </summary>
    public class LocalCacheManagerOptions
    {
        /// <summary>
        /// 配置名称，默认 "LocalCacheManagerConfig"
        /// </summary>
        public string ConfigName { get; set; } = "LocalCacheManagerConfig";

        /// <summary>
        /// 配置格式，默认 "ConfigFormat.XML"
        /// </summary>
        public ConfigFormat ConfigFormat { get; set; } = ConfigFormat.XML;
    }
}
