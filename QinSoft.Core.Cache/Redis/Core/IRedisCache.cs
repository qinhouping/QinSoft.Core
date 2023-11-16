using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Cache.Redis.Core
{
    /// <summary>
    /// Redis缓存
    /// </summary>
    public interface IRedisCache : IDatabase, IDisposable
    {
        /// <summary>
        /// 切换DB
        /// </summary>
        void Select(int database);
    }
}
