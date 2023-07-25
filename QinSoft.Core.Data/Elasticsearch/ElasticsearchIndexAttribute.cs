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
    /// elasticsearch上下文特性
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
            ObjectUtils.CheckNull(indexName, "indexName");
            this.IndexName = indexName;
        }

        public ElasticsearchIndexAttribute(string name, string indexName)
        {
            ObjectUtils.CheckNull(name, "name");
            ObjectUtils.CheckNull(indexName, "indexName");
            this.Name = name;
            this.IndexName = indexName;
        }
    }
}
