using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QinSoft.Core.Cache.Redis.Core
{
    /// <summary>
    /// Redis缓存池
    /// </summary>
    public interface IRedisCachePool : IDisposable
    {
        /// <summary>
        /// 获取Redis缓存
        /// </summary>
        IRedisCache Get();

        /// <summary>
        /// 获取Redis缓存
        /// </summary>
        Task<IRedisCache> GetAsync();
    }
}
