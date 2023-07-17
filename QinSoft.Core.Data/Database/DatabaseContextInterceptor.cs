using Castle.DynamicProxy;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using QinSoft.Core.Common.Utils;

namespace QinSoft.Core.Data.Database
{
    /// <summary>
    /// 数据上下文拦截器，支持多数据源切换
    /// </summary>
    public class DatabaseContextInterceptor : IInterceptor
    {
        /// <summary>
        /// 数据库管理器
        /// </summary>
        protected IDatabaseManager DatabaseManager { get; set; }

        /// <summary>
        /// 数据上下文栈
        /// </summary>
        protected IDatabaseContextStack DatabaseContextStack { get; set; }

        public DatabaseContextInterceptor(IDatabaseManager databaseManager, IDatabaseContextStack databaseContextStack)
        {
            ArgumentUtils.CheckNull(databaseManager, "databaseManager");
            ArgumentUtils.CheckNull(databaseContextStack, "databaseContextStack");
            this.DatabaseManager = databaseManager;
            this.DatabaseContextStack = databaseContextStack;
        }

        /// <summary>
        /// 拦截处理
        /// </summary>
        public virtual void Intercept(IInvocation invocation)
        {
            DatabaseContextAttribute databaseContext = GetDatabaseContext(invocation);
            if (databaseContext != null)
            {
                using (ISqlSugarClient client = string.IsNullOrEmpty(databaseContext.Name) ? this.DatabaseManager.GetDatabase() : this.DatabaseManager.GetDatabase(databaseContext.Name))
                {
                    DatabaseContextStack.Push(client);
                    try
                    {
                        if (client is SqlSugarClient && databaseContext.UseTran)
                        {
                            (client as SqlSugarClient).BeginTran();
                        }

                        //调用业务方法
                        invocation.Proceed();

                        if (client is SqlSugarClient && databaseContext.UseTran)
                        {
                            (client as SqlSugarClient).CommitTran();
                        }
                    }
                    catch (Exception e)
                    {
                        if (client is SqlSugarClient && databaseContext.UseTran)
                        {
                            (client as SqlSugarClient).RollbackTran();
                        }
                        throw e;
                    }
                    finally
                    {
                        DatabaseContextStack.Pop();
                    }
                }
            }
            else
            {
                //调用业务方法
                invocation.Proceed();
            }
        }

        /// <summary>
        /// 获取拦截上下文
        /// </summary>
        public virtual DatabaseContextAttribute GetDatabaseContext(IInvocation invocation)
        {
            DatabaseContextAttribute attribute = invocation.MethodInvocationTarget.GetCustomAttribute(typeof(DatabaseContextAttribute)) as DatabaseContextAttribute;
            if (attribute == null)
            {
                attribute = invocation.TargetType.GetCustomAttribute(typeof(DatabaseContextAttribute)) as DatabaseContextAttribute;
            }

            return attribute;
        }
    }
}
