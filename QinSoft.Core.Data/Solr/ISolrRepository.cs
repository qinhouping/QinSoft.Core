using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using QinSoft.Core.Common.Utils;
using System.Threading.Tasks;
using SolrNet;
using SolrNet.Commands.Parameters;
using System.Threading;

namespace QinSoft.Core.Data.Solr
{
    /// <summary>
    /// Solr仓库接口
    /// </summary>
    public interface ISolrRepository<T> where T : class, new()
    {
        /// <summary>
        /// 当前上下文Solr客户端
        /// </summary>
        ISolrOperations<T> Client { get; }

        /// <summary>
        /// 核心名
        /// </summary>
        string CoreName { get; }

        /// <summary>
        /// 保存文档
        /// </summary>
        bool Add(T document, AddParameters parameters = null);

        /// <summary>
        /// 保存文档
        /// </summary>
        bool AddRange(T[] documents, AddParameters parameters = null);

        /// <summary>
        /// 保存文档
        /// </summary>
        Task<bool> AddAsync(T document, AddParameters parameters = null);

        /// <summary>
        /// 保存文档
        /// </summary>
        Task<bool> AddRangeAsync(T[] documents, AddParameters parameters = null);

        /// <summary>
        /// 移除文档
        /// </summary>
        bool Delete(T document, DeleteParameters parameters = null);

        /// <summary>
        /// 移除文档
        /// </summary>
        bool Delete(T[] documents, DeleteParameters parameters = null);

        /// <summary>
        /// 移除文档
        /// </summary>
        bool Delete(string id, DeleteParameters parameters = null);

        /// <summary>
        /// 移除文档
        /// </summary>
        bool Delete(string[] ids, DeleteParameters parameters = null);

        /// <summary>
        /// 移除文档
        /// </summary>
        bool Delete(ISolrQuery query, DeleteParameters parameters = null);

        /// <summary>
        /// 移除文档
        /// </summary>
        Task<bool> DeleteAsync(T document, DeleteParameters parameters = null);

        /// <summary>
        /// 移除文档
        /// </summary>
        Task<bool> DeleteAsync(T[] documents, DeleteParameters parameters = null);

        /// <summary>
        /// 移除文档
        /// </summary>
        Task<bool> DeleteAsync(string id, DeleteParameters parameters = null);

        /// <summary>
        /// 移除文档
        /// </summary>
        Task<bool> DeleteAsync(string[] ids, DeleteParameters parameters = null);

        /// <summary>
        /// 移除文档
        /// </summary>
        Task<bool> DeleteAsync(ISolrQuery query, DeleteParameters parameters = null);

        /// <summary>
        /// 提交操作
        /// </summary>
        bool Commit();

        /// <summary>
        /// 提交操作
        /// </summary>
        Task<bool> CommitAsync();

        /// <summary>
        /// 查询文档
        /// </summary>
        IEnumerable<T> Query(string query, QueryOptions options = null);

        /// <summary>
        /// 查询文档
        /// </summary>
        IEnumerable<T> Query(ISolrQuery query, QueryOptions options = null);

        /// <summary>
        /// 查询文档
        /// </summary>
        IEnumerable<T> Query(string query, SortOrder[] sorts = null);

        /// <summary>
        /// 查询文档
        /// </summary>
        IEnumerable<T> Query(ISolrQuery query, SortOrder[] sorts = null);

        /// <summary>
        /// 查询文档
        /// </summary>
        Task<IEnumerable<T>> QueryAsync(string query, QueryOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 查询文档
        /// </summary>
        Task<IEnumerable<T>> QueryAsync(ISolrQuery query, QueryOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 查询文档
        /// </summary>
        Task<IEnumerable<T>> QueryAsync(string query, SortOrder[] sorts = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 查询文档
        /// </summary>
        Task<IEnumerable<T>> QueryAsync(ISolrQuery query, SortOrder[] sorts = null, CancellationToken cancellationToken = default);
    }
}
