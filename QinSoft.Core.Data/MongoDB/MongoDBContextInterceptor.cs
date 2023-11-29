using Castle.DynamicProxy;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using QinSoft.Core.Common.Utils;

namespace QinSoft.Core.Data.MongoDB
{
    /// <summary>
    /// mongodb拦截
    /// </summary>
    public class MongoDBContextInterceptor : IInterceptor
    {
        /// <summary>
        /// 数据库管理器
        /// </summary>
        protected IMongoDBManager MongoDBManager { get; set; }

        /// <summary>
        /// 数据上下文栈
        /// </summary>
        protected IMongoDBContext MongoDBContext { get; set; }

        public MongoDBContextInterceptor(IMongoDBManager mongodbManager, IMongoDBContext mongodbContext)
        {
            ObjectUtils.CheckNull(mongodbManager, nameof(mongodbManager));
            ObjectUtils.CheckNull(mongodbContext, nameof(mongodbContext));
            this.MongoDBManager = mongodbManager;
            this.MongoDBContext = mongodbContext;
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
