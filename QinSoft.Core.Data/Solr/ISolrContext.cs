using Nest;
using QinSoft.Core.Data.MongoDB.Core;
using SolrNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Data.Solr
{
    /// <summary>
    /// solr上下文
    /// </summary>
    public interface ISolrContext : IDisposable
    {
        /// <summary>
        /// 获取mongodb客户端
        /// </summary>
        ISolrOperations<T> Get<T>(string coreName);

        /// <summary>
        /// 获取mongodb客户端
        /// </summary>
        ISolrOperations<T> Get<T>(string name, string coreName);
    }
}
