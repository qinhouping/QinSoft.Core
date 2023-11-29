using QinSoft.Core.Common.Utils;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace QinSoft.Core.Data.Elasticsearch
{
    /// <summary>
    /// elasticsearch索引特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ElasticsearchIndexAttribute : Attribute
    {
        /// <summary>
        /// 上下文名称
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// 索引名称
        /// </summary>
        public string IndexName { get; private set; }

        public ElasticsearchIndexAttribute(string indexName)
        {
            ObjectUtils.CheckNull(indexName, nameof(indexName));
            this.IndexName = indexName;
        }

        public ElasticsearchIndexAttribute(string name, string indexName)
        {
            ObjectUtils.CheckNull(name, nameof(name));
            ObjectUtils.CheckNull(indexName, nameof(indexName));
            this.Name = name;
            this.IndexName = indexName;
        }
    }
}
