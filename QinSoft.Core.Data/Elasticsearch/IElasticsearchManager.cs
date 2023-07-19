using Nest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QinSoft.Core.Data.Elasticsearch
{
    public interface IElasticsearchManager : IDisposable
    {
        /// <summary>
        /// 获取elasticsearch客户端
        /// </summary>
        IElasticClient GetElasticsearch();


        /// <summary>
        /// 获取elasticsearch客户端
        /// </summary>
        Task<IElasticClient> GetElasticsearchAsync();

        /// <summary>
        /// 获取elasticsearch客户端
        /// </summary>
        IElasticClient GetElasticsearch(string name);

        /// <summary>
        /// 获取elasticsearch客户端
        /// </summary>
        Task<IElasticClient> GetElasticsearchAsync(string name);
    }
}
