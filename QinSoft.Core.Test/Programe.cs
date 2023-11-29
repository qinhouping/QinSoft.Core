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
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using NLog.Extensions.Logging;
using QinSoft.Core.Data.MongoDB;
using QinSoft.Core.Data.Elasticsearch;
using QinSoft.Core.Test;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using QinSoft.Core.Data.Solr;

namespace QinSoft.Core.Test
{
    [TestClass]
    public class Programe
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        [AssemblyInitialize]
        public static void Initialize(TestContext context)
        {
            //IOC
            IServiceCollection services = new ServiceCollection()
            .AddLogging(builder =>
            {
                //builder.AddNLog();
                //builder.AddLog4Net();
                builder.AddConsole();
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

            })
            .AddMongoDBManager(options =>
            {

            })
            .AddElasticsearchManager(options =>
            {

            })
            .AddKafkaManager(options =>
            {

            })
            .AddZookeeperManager(options =>
            {

            })
            .AddMQTTManager(options =>
            {

            })
            .AddSolrManager(options =>
            {

            })
            .AddRabbitMQManager(options =>
            {

            })
            .AddJobSchedule()
            .AddEventBus(builder =>
            {
                builder.AddMQTTChannel(options =>
                {

                });
            });

            services.TryProxyAddSingleton<IProjectRepository, ProjectRepository>(typeof(DatabaseContextInterceptor));
            services.TryProxyAddSingleton<IProjectMongoDBRepository, ProjectMongoDBRepository>(typeof(MongoDBContextInterceptor));
            services.TryProxyAddSingleton<IProjectElasticsearchRepository, ProjectElasticsearchRepository>(typeof(ElasticsearchContextInterceptor));
            services.TryProxyAddSingleton<IProjectSolrRepository, ProjectSolrRepository>(typeof(SolrContextInterceptor));

            services.AddTransient<TestJob>();

            ServiceProvider = services.BuildServiceProvider();
        }
    }
}
