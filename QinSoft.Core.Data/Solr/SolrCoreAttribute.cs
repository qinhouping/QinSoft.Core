using QinSoft.Core.Common.Utils;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace QinSoft.Core.Data.Solr
{
    /// <summary>
    /// solr核心特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SolrCoreAttribute : Attribute
    {
        /// <summary>
        /// 上下文名称
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// 核心名称
        /// </summary>
        public string CoreName { get; private set; }

        public SolrCoreAttribute(string coreName)
        {
            ObjectUtils.CheckNull(coreName, "coreName");
            this.CoreName = coreName;
        }

        public SolrCoreAttribute(string name, string coreName)
        {
            ObjectUtils.CheckNull(name, "name");
            ObjectUtils.CheckNull(coreName, "coreName");
            this.Name = name;
            this.CoreName = coreName;
        }
    }
}
