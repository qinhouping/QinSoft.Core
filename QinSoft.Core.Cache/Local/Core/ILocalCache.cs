﻿using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Cache.Local.Core
{
    /// <summary>
    /// 本地缓存接口
    /// </summary>
    public interface ILocalCache : IMemoryCache, ICache, IDisposable
    {

    }
}
