using QinSoft.Core.Cache.Local;
using QinSoft.Core.Common.Utils;
using QinSoft.Core.Configure;
using QinSoft.Core.Configure.FileConfiger;
using QinSoft.Core.Data.Database;
using log4net.Config;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;

namespace QinSoft.Core.Test
{
    [TestClass]
    public class Programe
    {
        public static IServiceProvider ServiceProvider;

        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            //IOC
            IServiceCollection services = new ServiceCollection()
            .AddLogging(builder =>
            {
                //builder.AddLog4Net();
            })
            .AddFileConfiger(options =>
            {
                options.ExpireIn = 600;
            })
            .AddLocalCacheManager(options =>
            {

            })
            .AddRedisCacheManager(options =>
            {

            })
            .AddDatabaseManager(options =>
            {

            });

            services.TryProxyAddTransient<ITestTableRepository, TestTableRepository>(typeof(DatabaseContextInterceptor));
            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
