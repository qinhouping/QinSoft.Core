﻿using QinSoft.Core.Data.MongoDB.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SolrNet;

namespace QinSoft.Core.Data.Solr
{
    public interface ISolrManager : IDisposable
    {
        /// <summary>
        /// 获取Solr客户端
        /// </summary>
        ISolrOperations<T> GetSolr<T>(string coreName);

        /// <summary>
        /// 获取Solr客户端
        /// </summary>
        Task<ISolrOperations<T>> GetSolrAsync<T>(string coreName);

        /// <summary>
        /// 获取Solr客户端
        /// </summary>
        ISolrOperations<T> GetSolr<T>(string name,string coreName);

        /// <summary>
        /// 获取Solr客户端
        /// </summary>
        Task<ISolrOperations<T>> GetSolrAsync<T>(string name, string coreName);
    }
}
