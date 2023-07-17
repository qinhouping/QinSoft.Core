using QinSoft.Core.Cache.Redis.Core;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Cache.Redis
{
    /// <summary>
    /// Redis缓存管理器接口
    /// </summary>
    public interface IRedisCacheManager : CacheManager<IRedisCache>
    {
    }
}
