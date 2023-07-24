using Nest;
using QinSoft.Core.Data.MongoDB.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Data.Elasticsearch
{
    /// <summary>
    /// mongodb上下文
    /// </summary>
    public interface IElasticsearchContext : IDisposable
    {
        /// <summary>
        /// 获取mongodb客户端
        /// </summary>
        IElasticClient Get();

        /// <summary>
        /// 获取mongodb客户端
        /// </summary>
        IElasticClient Get(string name);
    }
}
