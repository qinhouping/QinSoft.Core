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
    /// 数据库上下文接口
    /// </summary>
    public interface IDatabaseContextStack : IDisposable
    {

        /// <summary>
        /// 入栈
        /// </summary>
        void Push(ISqlSugarClient client);

        /// <summary>
        /// 出栈
        /// </summary>
        ISqlSugarClient Pop();

        /// <summary>
        /// 出栈（不移除）
        /// </summary>
        ISqlSugarClient Peek();
    }
}
