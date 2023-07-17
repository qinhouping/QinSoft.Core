using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QinSoft.Core.Cache
{
    /// <summary>
    /// 缓存管理器接口
    /// </summary>
    public interface CacheManager<T> : IDisposable
    {
        /// <summary>
        /// 获取缓存
        /// </summary>
        T GetCache();

        /// <summary>
        /// 获取缓存
        /// </summary>
        Task<T> GetCacheAsync();

        /// <summary>
        /// 获取缓存
        /// </summary>
        T GetCache(string name);

        /// <summary>
        /// 获取缓存
        /// </summary>
        Task<T> GetCacheAsync(string name);
    }
}
