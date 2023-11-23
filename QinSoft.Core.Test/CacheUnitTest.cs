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
using QinSoft.Core.Cache.CSRedis;
using CSRedis;
using QinSoft.Core.Cache.CSRedis.Core;

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
                ILocalCache cache = localCacheManager.GetCache("test");
                string value = Guid.NewGuid().ToString();
                cache.Set("test", value);
                Assert.AreEqual(cache.Get<string>("test"), value);
                cache.Remove("test");
            }
        }

        [TestMethod]
        public void TestRedisCache()
        {
            IConfiger configer = new FileConfiger(new FileConfigerOptions());
            RedisCacheManagerConfig redisCacheConfig = configer.Get<RedisCacheManagerConfig>("RedisCacheManagerConfig");

            using (IRedisCacheManager redisCacheManager = new RedisCacheManager(redisCacheConfig))
            {
                IRedisCache cache = redisCacheManager.GetCache("test");
                string value = Guid.NewGuid().ToString();
                cache.StringSet("test", value);
                Assert.AreEqual(cache.StringGet("test").ToString(), value);
                cache.KeyDelete("test");
            }
        }

        [TestMethod]
        public void TestCSRedisCache()
        {
            IConfiger configer = new FileConfiger(new FileConfigerOptions());
            CSRedisCacheManagerConfig redisCacheConfig = configer.Get<CSRedisCacheManagerConfig>("CSRedisCacheManagerConfig");

            using (ICSRedisCacheManager redisCacheManager = new CSRedisCacheManager(redisCacheConfig))
            {
                CSRedisCache cache = redisCacheManager.GetCache("test");
                string value = Guid.NewGuid().ToString();
                cache.Set("test", value);
                Assert.AreEqual(cache.Get("test").ToString(), value);
                cache.Del("test");
            }
        }
    }
}
