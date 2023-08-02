using StackExchange.Redis;
using StackExchange.Redis.MultiplexerPool;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace QinSoft.Core.Cache.Redis.Core
{
    /// <summary>
    /// Redis缓存连接池配置选项
    /// </summary>
    public class RedisCachePoolOptions
    {
        /// <summary>
        /// 连接池大小
        /// </summary>
        public int PoolSize { get; set; } = 50;

        /// <summary>
        /// 连接配置选项
        /// </summary>
        public ConfigurationOptions ConfigurationOptions { get; set; }

        /// <summary>
        /// 日志输出
        /// </summary>
        public TextWriter TextWriter { get; set; }

        /// <summary>
        /// 连接选择策略
        /// </summary>
        public ConnectionSelectionStrategy ConnectionSelectionStrategy { get; set; }
    }
}
