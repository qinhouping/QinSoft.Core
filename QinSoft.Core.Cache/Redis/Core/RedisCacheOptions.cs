using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Cache.Redis.Core
{
    /// <summary>
    /// Redis缓存项
    /// </summary>
    public class RedisCacheOptions
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string Configuration { get; set; }

        /// <summary>
        /// 连接配置选项，该项比"Configuration"优先
        /// </summary>
        public ConfigurationOptions ConfigurationOptions { get; set; }
    }
}
