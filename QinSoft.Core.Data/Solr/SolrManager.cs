using QinSoft.Core.Common.Utils;
using QinSoft.Core.Configure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using SolrNet;
using SolrNet.Impl;
using HttpWebAdapters;
using CommonServiceLocator;
using System.IO;

namespace QinSoft.Core.Data.Solr
{
    /// <summary>
    /// Solr管理器
    /// 集成缓存配置，支持多数据源
    /// </summary>
    public class SolrManager : ISolrManager
    {
        /// <summary>
        /// 日志
        /// </summary>
        protected ILogger logger;

        /// <summary>
        /// Solr配置
        /// </summary>
        public SolrManagerConfig SolrManagerConfig { get; private set; }

        /// <summary>
        /// 缓存字典
        /// </summary>
        protected ConcurrentDictionary<string, object> CacheDictionary;

        public SolrManager(SolrManagerConfig config) : this(config, NullLoggerFactory.Instance.CreateLogger<SolrManager>())
        {

        }

        public SolrManager(SolrManagerConfig config, ILogger logger)
        {
            ObjectUtils.CheckNull(config, "config");
            ObjectUtils.CheckNull(logger, "logger");
            this.CacheDictionary = new ConcurrentDictionary<string, object>();
            SolrManagerConfig = config;
            this.logger = logger;
        }

        public SolrManager(SolrManagerOptions options, IConfiger configer) : this(options, configer, NullLoggerFactory.Instance)
        {
        }

        public SolrManager(SolrManagerOptions options, IConfiger configer, ILoggerFactory loggerFactory)
        {
            ObjectUtils.CheckNull(options, "options");
            ObjectUtils.CheckNull(configer, "configer");
            ObjectUtils.CheckNull(loggerFactory, "loggerFactory");
            this.CacheDictionary = new ConcurrentDictionary<string, object>();
            SolrManagerConfig = configer.Get<SolrManagerConfig>(options.ConfigName, options.ConfigFormat);
            logger = loggerFactory.CreateLogger<SolrManager>();
        }

        public SolrManager(IOptions<SolrManagerOptions> optionsAccessor, IConfiger configer) : this(optionsAccessor, configer, NullLoggerFactory.Instance)
        {
        }

        public SolrManager(IOptions<SolrManagerOptions> optionsAccessor, IConfiger configer, ILoggerFactory loggerFactory)
        {
            ObjectUtils.CheckNull(optionsAccessor, "optionsAccessor");
            ObjectUtils.CheckNull(configer, "configer");
            ObjectUtils.CheckNull(loggerFactory, "loggerFactory");
            this.CacheDictionary = new ConcurrentDictionary<string, object>();
            SolrManagerConfig = configer.Get<SolrManagerConfig>(optionsAccessor.Value.ConfigName, optionsAccessor.Value.ConfigFormat);
            logger = loggerFactory.CreateLogger<SolrManager>();
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        protected virtual SolrItemConfig GetSolrItemConfig(string name)
        {
            ObjectUtils.CheckNull(name, name);
            return SolrManagerConfig.GetByName(name);
        }

        /// <summary>
        /// 获取默认配置
        /// </summary>
        protected virtual SolrItemConfig GetDefaultSolrItemConfig()
        {
            return GetSolrItemConfig(this.SolrManagerConfig.Primary);
        }

        /// <summary>
        /// 构建Solr客户端实例
        /// </summary>
        protected virtual ISolrOperations<T> BuildClientFromConfig<T>(SolrItemConfig config,string coreName)
        {
            ObjectUtils.CheckNull(config, "config");
            SolrConnection connection = new SolrConnection(Path.Combine(config.Url,coreName));
            if (!config.Username.IsEmpty() && !config.Password.IsEmpty())
            {
                connection.HttpWebRequestFactory = new BasicAuthHttpWebRequestFactory(config.Username, config.Password);
            }
            Startup.Init<T>(connection);
            var solr = ServiceLocator.Current.GetInstance<ISolrOperations<T>>();
            return solr;
        }

        /// <summary>
        /// 获取Solr客户端
        /// </summary>
        public virtual ISolrOperations<T> GetSolr<T>(string coreName)
        {
            ObjectUtils.CheckNull(coreName, "coreName");
            SolrItemConfig config = GetDefaultSolrItemConfig();
            if (config == null)
            {
                throw new SolrException("not found default Solr client config");
            }

            ISolrOperations<T> client =(ISolrOperations<T>) CacheDictionary.GetOrAdd(config.Name+":"+coreName, key => BuildClientFromConfig<T>(config,coreName));

            logger.LogDebug("get default Solr client from config");

            return client;
        }

        /// <summary>
        /// 获取Solr客户端
        /// </summary>
        public virtual async Task<ISolrOperations<T>> GetSolrAsync<T>(string coreName)
        {
            return await ExecuteUtils.ExecuteInTask(GetSolr<T>,coreName);
        }

        /// <summary>
        /// 获取Solr客户端
        /// </summary>
        public virtual ISolrOperations<T> GetSolr<T>(string name, string coreName)
        {
            ObjectUtils.CheckNull(name, "name");
            ObjectUtils.CheckNull(coreName, "coreName");
            SolrItemConfig config = GetSolrItemConfig(name);
            if (config == null)
            {
                throw new SolrException(string.Format("not found Solr client config:{0}", name));
            }

            ISolrOperations<T> client = (ISolrOperations<T>)CacheDictionary.GetOrAdd(config.Name + ":" + coreName, key => BuildClientFromConfig<T>(config,coreName));

            logger.LogDebug(string.Format("get Solr client from config:{0}", name));

            return client;
        }

        /// <summary>
        /// 获取Solr客户端
        /// </summary>
        public virtual async Task<ISolrOperations<T>> GetSolrAsync<T>(string name, string coreName)
        {
            return await ExecuteUtils.ExecuteInTask(()=>GetSolr<T>(name,coreName));
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            Startup.Container.Clear();
            GC.SuppressFinalize(this);
        }
    }
}
