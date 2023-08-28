using QinSoft.Core.Cache.Local;
using QinSoft.Core.Cache.Local.Core;
using QinSoft.Core.Cache.Redis;
using QinSoft.Core.Cache.Redis.Core;
using QinSoft.Core.Configure;
using QinSoft.Core.Configure.FileConfiger;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Test
{
    [TestClass]
    public class CacheUnitTest
    {
        [TestMethod]
        public void TestMemoryCache()
        {
            IConfiger configer = new FileConfiger(new FileConfigerOptions());
            LocalCacheManagerConfig LocalCacheConfig = configer.Get<LocalCacheManagerConfig>("LocalCacheManagerConfig");

            using (ILocalCacheManager localCacheManager = new LocalCacheManager(LocalCacheConfig))
            {

                using (ILocalCache cache = localCacheManager.GetCache("test"))
                {
                    string value = Guid.NewGuid().ToString();
                    cache.Set("test", value);
                    Assert.AreEqual(cache.Get<string>("test"), value);
                    cache.Remove("test");
                }
            }
        }

        [TestMethod]
        public void TestRedisCache()
        {
            IConfiger configer = new FileConfiger(new FileConfigerOptions());
            RedisCacheManagerConfig redisCacheConfig = configer.Get<RedisCacheManagerConfig>("RedisCacheManagerConfig");

            using (IRedisCacheManager redisCacheManager = new RedisCacheManager(redisCacheConfig))
            {
                using (IRedisCache cache = redisCacheManager.GetCache("test"))
                {
                    string value = Guid.NewGuid().ToString();
                    cache.StringSet("test", value);
                    Assert.AreEqual(cache.StringGet("test").ToString(), value);
                    cache.KeyDelete("test");
                }
            }
        }
    }
}
