using QinSoft.Core.Cache.Redis.Core;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;
using CSRedis;
using QinSoft.Core.Cache.CSRedis.Core;

namespace QinSoft.Core.Cache.CSRedis
{
    /// <summary>
    /// Redis缓存管理器接口
    /// </summary>
    public interface ICSRedisCacheManager : CacheManager<CSRedisCache>
    {
    }
}
