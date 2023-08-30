using Nest;
using QinSoft.Core.Common.Utils;
using SolrNet;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace QinSoft.Core.Data.Solr
{
    /// <summary>
    /// Solr上下文
    /// </summary>
    public class SolrContext : ISolrContext
    {
        /// </summary>
        /// 单例模式，内部使用
        /// </summary>
        internal static ISolrContext Instance { get; set; }

        protected ISolrManager SolrManager { get; set; }

        protected AsyncLocal<ConcurrentDictionary<string, object>> Context { get; set; }

        /// <summary>
        /// 默认上下文
        /// </summary>
        protected AsyncLocal<ConcurrentDictionary<string, object>> DefaultContext { get; set; }

        public SolrContext(ISolrManager solrManager)
        {
            ObjectUtils.CheckNull(solrManager, "solrManager");
            this.Context = new AsyncLocal<ConcurrentDictionary<string, object>>();
            this.DefaultContext = new AsyncLocal<ConcurrentDictionary<string, object>>();
            this.SolrManager = solrManager;
            Instance = this;
        }

        /// <summary>
        /// 获取mongodb客户端
        /// </summary>
        public virtual ISolrOperations<T> Get<T>(string coreName)
        {
            if (DefaultContext.Value == null)
            {
                DefaultContext.Value = new ConcurrentDictionary<string, object>();
            }
            return (ISolrOperations<T>)DefaultContext.Value.GetOrAdd(coreName, SolrManager.GetSolr<T>(coreName));
        }

        /// <summary>
        /// 获取mongodb客户端
        /// </summary>
        public virtual ISolrOperations<T> Get<T>(string name,string coreName)
        {
            if (Context.Value == null)
            {
                Context.Value = new ConcurrentDictionary<string, object>();
            }
            return (ISolrOperations<T>) Context.Value.GetOrAdd(name+":"+coreName, SolrManager.GetSolr<T>(name, coreName));
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
