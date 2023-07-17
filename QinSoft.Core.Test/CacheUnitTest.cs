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
                    cache.Set("test", "value");
                }

                using (ILocalCache cache2 = localCacheManager.GetCache("test"))
                {
                    Assert.AreEqual(cache2.Get<string>("test"), "value");
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

                string time = DateTime.Now.ToString();

                using (IRedisCache cache = redisCacheManager.GetCache("test"))
                {
                    cache.StringSet("now", time);
                }

                using (IRedisCache cache2 = redisCacheManager.GetCache("test"))
                {
                    Assert.AreEqual(cache2.StringGet("now").ToString(), time);
                }
            }
        }
    }
}
