using Elasticsearch.Net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Nest;
using QinSoft.Core.Common.Utils;
using QinSoft.Core.Configure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace QinSoft.Core.Data.Elasticsearch
{
    public class ElasticsearchManager : IElasticsearchManager
    {
        /// <summary>
        /// 缓存字典
        /// </summary>
        protected ConcurrentDictionary<string, IElasticClient> CacheDictionary;

        /// <summary>
        /// 日志
        /// </summary>
        protected ILogger logger;

        /// <summary>
        /// Database配置
        /// </summary>
        public ElasticsearchManagerConfig ElasticsearchManagerConfig { get; private set; }

        public ElasticsearchManager(ElasticsearchManagerConfig config) : this(config, NullLoggerFactory.Instance.CreateLogger<ElasticsearchManager>())
        {

        }

        public ElasticsearchManager(ElasticsearchManagerConfig config, ILogger logger)
        {
            ObjectUtils.CheckNull(config, "config");
            ObjectUtils.CheckNull(logger, "logger");
            CacheDictionary = new ConcurrentDictionary<string, IElasticClient>();
            ElasticsearchManagerConfig = config;
            this.logger = logger;
        }

        public ElasticsearchManager(ElasticsearchManagerOptions options, IConfiger configer) : this(options, configer, NullLoggerFactory.Instance)
        {
        }

        public ElasticsearchManager(ElasticsearchManagerOptions options, IConfiger configer, ILoggerFactory loggerFactory)
        {
            ObjectUtils.CheckNull(options, "options");
            ObjectUtils.CheckNull(configer, "configer");
            ObjectUtils.CheckNull(loggerFactory, "loggerFactory");
            CacheDictionary = new ConcurrentDictionary<string, IElasticClient>();
            ElasticsearchManagerConfig = configer.Get<ElasticsearchManagerConfig>(options.ConfigName, options.ConfigFormat);
            logger = loggerFactory.CreateLogger<ElasticsearchManager>();
        }

        public ElasticsearchManager(IOptions<ElasticsearchManagerOptions> optionsAccessor, IConfiger configer) : this(optionsAccessor, configer, NullLoggerFactory.Instance)
        {
        }

        public ElasticsearchManager(IOptions<ElasticsearchManagerOptions> optionsAccessor, IConfiger configer, ILoggerFactory loggerFactory)
        {
            ObjectUtils.CheckNull(optionsAccessor, "optionsAccessor");
            ObjectUtils.CheckNull(configer, "configer");
            ObjectUtils.CheckNull(loggerFactory, "loggerFactory");
            CacheDictionary = new ConcurrentDictionary<string, IElasticClient>();
            ElasticsearchManagerConfig = configer.Get<ElasticsearchManagerConfig>(optionsAccessor.Value.ConfigName, optionsAccessor.Value.ConfigFormat);
            logger = loggerFactory.CreateLogger<ElasticsearchManager>();
        }

        /// <summary>
        /// 获取缓存配置
        /// </summary>
        protected virtual ElasticsearchItemConfig GetElasticsearchItemConfig(string name)
        {
            ObjectUtils.CheckNull(name, name);
            return ElasticsearchManagerConfig.GetByName(name);
        }

        /// <summary>
        /// 获取默认缓存配置
        /// </summary>
        protected virtual ElasticsearchItemConfig GetDefaultElasticsearchItemConfig()
        {
            return GetElasticsearchItemConfig(this.ElasticsearchManagerConfig.Primary);
        }

        /// <summary>
        /// 构建elasticsearch客户端实例
        /// </summary>
        protected virtual IElasticClient BuildClientFromConfig(ElasticsearchItemConfig config)
        {
            ObjectUtils.CheckNull(config, "config");
            SniffingConnectionPool pool = new SniffingConnectionPool(config.Urls.Select(u => new Uri(u)));
            ConnectionSettings connectionSettings = new ConnectionSettings(pool);
            if (!config.ApiKey.IsEmpty())
            {
                connectionSettings.ApiKeyAuthentication(new ApiKeyAuthenticationCredentials(config.ApiKey));
            }
            if (!config.Username.IsEmpty() && !config.Password.IsEmpty())
            {
                connectionSettings.BasicAuthentication(config.Username, config.Password);
            }
            if (!config.DefaultIndexName.IsEmpty())
            {
                connectionSettings.DefaultIndex(config.DefaultIndexName);
            }
            connectionSettings.RequestTimeout(TimeSpan.FromMilliseconds(config.RequestTimeout));
            return new ElasticClient(connectionSettings);
        }

        /// <summary>
        /// 获取elasticsearch客户端
        /// </summary>
        public virtual IElasticClient GetElasticsearch()
        {
            ElasticsearchItemConfig config = GetDefaultElasticsearchItemConfig();
            if (config == null)
            {
                throw new ElasticsearchException("not found default elasticsearch client config");
            }

            IElasticClient client = CacheDictionary.GetOrAdd(config.Name, key => BuildClientFromConfig(config));

            logger.LogDebug("get default elasticsearch client from config");

            return client;
        }


        /// <summary>
        /// 获取elasticsearch客户端
        /// </summary>
        public virtual async Task<IElasticClient> GetElasticsearchAsync()
        {
            return await ExecuteUtils.ExecuteInTask(GetElasticsearch);
        }

        /// <summary>
        /// 获取elasticsearch客户端
        /// </summary>
        public virtual IElasticClient GetElasticsearch(string name)
        {
            ObjectUtils.CheckNull(name, "name");
            ElasticsearchItemConfig config = GetElasticsearchItemConfig(name);
            if (config == null)
            {
                throw new ElasticsearchException(string.Format("not found elasticsearch client config:{0}", name));
            }

            IElasticClient client = CacheDictionary.GetOrAdd(config.Name, key => BuildClientFromConfig(config));

            logger.LogDebug(string.Format("get elasticsearch client from config:{0}", name));

            return client;
        }

        /// <summary>
        /// 获取elasticsearch客户端
        /// </summary>
        public virtual async Task<IElasticClient> GetElasticsearchAsync(string name)
        {
            return await ExecuteUtils.ExecuteInTask(GetElasticsearch, name);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
            if (CacheDictionary != null)
            {
                CacheDictionary.Clear();
            }
        }
    }
}
