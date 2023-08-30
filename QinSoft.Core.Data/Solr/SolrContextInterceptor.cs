using Castle.DynamicProxy;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using QinSoft.Core.Common.Utils;

namespace QinSoft.Core.Data.Solr
{
    /// <summary>
    /// Solr上下文拦截器
    /// </summary>
    public class SolrContextInterceptor : IInterceptor
    {
        /// <summary>
        /// 数据库管理器
        /// </summary>
        protected ISolrManager SolrManager { get; set; }

        /// <summary>
        /// 数据上下文栈
        /// </summary>
        protected ISolrContext SolrContext { get; set; }

        public SolrContextInterceptor(ISolrManager solrManager, ISolrContext solrContext)
        {
            ObjectUtils.CheckNull(solrManager, "solrManager");
            ObjectUtils.CheckNull(solrContext, "solrContext");
            this.SolrManager = solrManager;
            this.SolrContext = solrContext;
        }

        /// <summary>
        /// 拦截处理
        /// </summary>
        public virtual void Intercept(IInvocation invocation)
        {
            //调用业务方法
            invocation.Proceed();
        }
    }
}
