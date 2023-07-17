using QinSoft.Core.Common.Utils;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace QinSoft.Core.Data.Database
{
    /// <summary>
    /// 上下文栈
    /// 必现单例注入
    /// </summary>
    public class DatabaseContextStack : IDatabaseContextStack
    {
        /// <summary>
        /// 单例模式，内部使用
        /// </summary>
        internal static IDatabaseContextStack Instance { get; set; }

        /// <summary>
        /// 上下文栈
        /// </summary>
        protected AsyncLocal<ConcurrentStack<ISqlSugarClient>> ContextStack { get; set; }

        /// <summary>
        /// 默认上下文
        /// </summary>
        protected AsyncLocal<ISqlSugarClient> DefaultContext { get; set; }

        /// <summary>
        /// 数据库管理器
        /// </summary>
        protected IDatabaseManager DatabaseManager { get; set; }

        public DatabaseContextStack(IDatabaseManager databaseManager)
        {
            ObjectUtils.CheckNull(databaseManager, "databaseManager");
            this.ContextStack = new AsyncLocal<ConcurrentStack<ISqlSugarClient>>();
            this.DefaultContext = new AsyncLocal<ISqlSugarClient>();
            this.DatabaseManager = databaseManager;
            Instance = this;
        }

        /// <summary>
        /// 入栈
        /// </summary>
        public virtual void Push(ISqlSugarClient client)
        {
            ObjectUtils.CheckNull(client, "client");

            if (ContextStack.Value == null)
            {
                ContextStack.Value = new ConcurrentStack<ISqlSugarClient>();
            }
            ContextStack.Value.Push(client);
        }

        /// <summary>
        /// 出栈
        /// </summary>
        public virtual ISqlSugarClient Pop()
        {
            if (ContextStack.Value == null)
            {
                return null;
            }
            ContextStack.Value.TryPop(out ISqlSugarClient client);
            if (ContextStack.Value.IsEmpty())
            {
                ContextStack.Value = null;
            }
            return client;
        }

        /// <summary>
        /// 出栈（不移除）
        /// </summary>
        public virtual ISqlSugarClient Peek()
        {
            if (ContextStack.Value == null || ContextStack.Value.IsEmpty())
            {
                //使用默认上下文
                if (DefaultContext.Value == null)
                {
                    DefaultContext.Value = this.DatabaseManager.GetDatabase();
                }
                return DefaultContext.Value;
            }
            ContextStack.Value.TryPeek(out ISqlSugarClient client);
            return client;
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
