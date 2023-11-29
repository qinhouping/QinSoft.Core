using Castle.DynamicProxy;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using QinSoft.Core.Common.Utils;

namespace QinSoft.Core.Data.Elasticsearch
{
    /// <summary>
    /// elasticsearch上下文拦截器
    /// </summary>
    public class ElasticsearchContextInterceptor : IInterceptor
    {
        /// <summary>
        /// 数据库管理器
        /// </summary>
        protected IElasticsearchManager ElasticsearchManager { get; set; }

        /// <summary>
        /// 数据上下文栈
        /// </summary>
        protected IElasticsearchContext ElasticsearchContext { get; set; }

        public ElasticsearchContextInterceptor(IElasticsearchManager elasticsearchManager, IElasticsearchContext elasticsearchContext)
        {
            ObjectUtils.CheckNull(elasticsearchManager, nameof(elasticsearchManager));
            ObjectUtils.CheckNull(elasticsearchContext, nameof(elasticsearchContext));
            this.ElasticsearchManager = elasticsearchManager;
            this.ElasticsearchContext = elasticsearchContext;
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
