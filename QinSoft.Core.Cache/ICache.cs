using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Cache
{
    public interface ICache
    {
        bool Set<T>(string key, T value, TimeSpan? timeSpan = null) where T : class;

        T GetT<T>(string key, Func<string, T> getValue = null, TimeSpan? timeSpan = null) where T : class;

        bool Delete(string key);
    }
}
