using QinSoft.Core.Cache.Local.Core;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QinSoft.Core.Cache.Local
{
    /// <summary>
    /// 本地缓存管理器接口
    /// </summary>
    public interface ILocalCacheManager : CacheManager<ILocalCache>
    {
    }
}
