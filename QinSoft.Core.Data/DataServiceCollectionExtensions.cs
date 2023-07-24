﻿using QinSoft.Core.Common.Utils;
using QinSoft.Core.Data.Database;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using QinSoft.Core.Data.MongoDB;
using QinSoft.Core.Data.Elasticsearch;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 数据服务依赖注入扩展
    /// </summary>
    public static class DataServiceCollectionExtensions
    {
        /// <summary>
        /// 注入Database
        /// </summary>
        public static IServiceCollection AddDatabaseManager(this IServiceCollection services)
        {
            ObjectUtils.CheckNull(services, "services");
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IDatabaseManager, DatabaseManager>());
            services.TryAdd(ServiceDescriptor.Singleton<IDatabaseContextStack, DatabaseContextStack>());
            services.TryAddSingleton<DatabaseContextInterceptor>();
            return services;
        }

        /// <summary>
        /// 注入Database
        /// </summary>
        public static IServiceCollection AddDatabaseManager(this IServiceCollection services, Action<DatabaseManagerOptions> setupAction)
        {
            ObjectUtils.CheckNull(services, "services");
            ObjectUtils.CheckNull(setupAction, "setupAction");
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IDatabaseManager, DatabaseManager>());
            services.TryAdd(ServiceDescriptor.Singleton<IDatabaseContextStack, DatabaseContextStack>());
            services.TryAddSingleton<DatabaseContextInterceptor>();
            services.Configure(setupAction);
            return services;
        }

        /// <summary>
        /// 注入MongoDB
        /// </summary>
        public static IServiceCollection AddMongoDBManager(this IServiceCollection services)
        {
            ObjectUtils.CheckNull(services, "services");
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IMongoDBManager, IMongoDBManager>());
            services.TryAdd(ServiceDescriptor.Singleton<IMongoDBContext, MongoDBContext>());
            services.TryAddSingleton<MongoDBContextInterceptor>();
            return services;
        }

        /// <summary>
        /// 注入MongoDB
        /// </summary>
        public static IServiceCollection AddMongoDBManager(this IServiceCollection services, Action<MongoDBManagerOptions> setupAction)
        {
            ObjectUtils.CheckNull(services, "services");
            ObjectUtils.CheckNull(setupAction, "setupAction");
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IMongoDBManager, MongoDBManager>());
            services.TryAdd(ServiceDescriptor.Singleton<IMongoDBContext, MongoDBContext>());
            services.TryAddSingleton<MongoDBContextInterceptor>();
            services.Configure(setupAction);
            return services;
        }

        /// <summary>
        /// 注入Elasticsearch
        /// </summary>
        public static IServiceCollection AddElasticsearchManager(this IServiceCollection services)
        {
            ObjectUtils.CheckNull(services, "services");
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IElasticsearchManager, ElasticsearchManager>());
            services.TryAdd(ServiceDescriptor.Singleton<IElasticsearchContext, ElasticsearchContext>());
            services.TryAddSingleton<ElasticsearchContextInterceptor>();
            return services;
        }

        /// <summary>
        /// 注入Elasticsearch
        /// </summary>
        public static IServiceCollection AddElasticsearchManager(this IServiceCollection services, Action<ElasticsearchManagerOptions> setupAction)
        {
            ObjectUtils.CheckNull(services, "services");
            ObjectUtils.CheckNull(setupAction, "setupAction");
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IElasticsearchManager, ElasticsearchManager>());
            services.TryAdd(ServiceDescriptor.Singleton<IElasticsearchContext, ElasticsearchContext>());
            services.TryAddSingleton<ElasticsearchContextInterceptor>();
            services.Configure(setupAction);
            return services;
        }
    }
}
