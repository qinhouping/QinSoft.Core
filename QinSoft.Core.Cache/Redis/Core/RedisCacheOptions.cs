using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Cache.Redis.Core
{
    /// <summary>
    /// Redis缓存配置选项
    /// </summary>
    public class RedisCacheOptions
    {
        /// <summary>
        /// 连接配置选项
        /// </summary>
        public ConfigurationOptions ConfigurationOptions { get; set; }

        /// <summary>
        /// 是否是哨兵模式
        /// </summary>
        public bool IsSentinel { get; set; }
    }
}
