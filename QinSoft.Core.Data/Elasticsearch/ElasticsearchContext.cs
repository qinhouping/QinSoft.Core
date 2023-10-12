using Nest;
using QinSoft.Core.Common.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace QinSoft.Core.Data.Elasticsearch
{
    /// <summary>
    /// elasticsearch上下文
    /// </summary>
    public class ElasticsearchContext : IElasticsearchContext
    {
        /// </summary>
        /// 单例模式，内部使用
        /// </summary>
        internal static IElasticsearchContext Instance { get; set; }

        protected IElasticsearchManager ElasticsearchManager { get; set; }

        protected AsyncLocal<ConcurrentDictionary<string, IElasticClient>> Context { get; set; }

        /// <summary>
        /// 默认上下文
        /// </summary>
        protected AsyncLocal<IElasticClient> DefaultContext { get; set; }

        public ElasticsearchContext(IElasticsearchManager elasticsearchManager)
        {
            ObjectUtils.CheckNull(elasticsearchManager, "elasticsearchManager");
            this.Context = new AsyncLocal<ConcurrentDictionary<string, IElasticClient>>();
            this.DefaultContext = new AsyncLocal<IElasticClient>();
            this.ElasticsearchManager = elasticsearchManager;
            Instance = this;
        }

        /// <summary>
        /// 获取mongodb客户端
        /// </summary>
        public virtual IElasticClient Get()
        {
            if (DefaultContext.Value == null)
            {
                DefaultContext.Value = ElasticsearchManager.GetElasticsearch();
            }
            return DefaultContext.Value;
        }

        /// <summary>
        /// 获取mongodb客户端
        /// </summary>
        public virtual IElasticClient Get(string name)
        {
            if (Context.Value == null)
            {
                Context.Value = new ConcurrentDictionary<string, IElasticClient>();
            }
            return Context.Value.GetOrAdd(name, key => ElasticsearchManager.GetElasticsearch(key));
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
