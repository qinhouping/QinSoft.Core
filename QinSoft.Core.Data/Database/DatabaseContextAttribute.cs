using QinSoft.Core.Common.Utils;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace QinSoft.Core.Data.Database
{
    /// <summary>
    /// 数据库上下文特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DatabaseContextAttribute : Attribute
    {
        /// <summary>
        /// 上下文名称
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// 是否使用事务
        /// </summary>
        public bool UseTran { get; private set; }

        public DatabaseContextAttribute(string name, bool useTran = false)
        {
            ObjectUtils.CheckNull(name, nameof(name));
            this.Name = name;
            this.UseTran = useTran;
        }

        public DatabaseContextAttribute(bool useTran = false)
        {
            this.UseTran = useTran;
        }
    }
}
