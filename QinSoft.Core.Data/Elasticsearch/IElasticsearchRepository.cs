using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using QinSoft.Core.Common.Utils;
using System.Threading.Tasks;
using Nest;

namespace QinSoft.Core.Data.Elasticsearch
{
    /// <summary>
    /// elasticsearch仓库接口
    /// </summary>
    public interface IElasticsearchRepository<T> where T : class, new()
    {
        /// <summary>
        /// 索引文档
        /// </summary>
        (bool, string) Index(T document);

        /// <summary>
        /// 索引文档
        /// </summary>
        Task<(bool, string)> IndexAsync(T document);

        /// <summary>
        /// 批量索引文档
        /// </summary>
        int BulkIndex(params T[] documents);

        /// <summary>
        /// 批量索引文档
        /// </summary>
        Task<int> BulkIndexAsync(params T[] documents);

        /// <summary>
        /// 移除文档
        /// </summary>
        bool Delete(string id);

        /// <summary>
        /// 移除文档
        /// </summary>
        Task<bool> DeleteAsync(string id);

        /// <summary>
        /// 移除文档
        /// </summary>
        int BulkDelete(params string[] ids);

        /// <summary>
        /// 移除文档
        /// </summary>
        Task<int> BulkDeleteAsync(params string[] ids);

        /// <summary>
        /// 更新文档
        /// </summary>
        bool Update(string id, T docuemnt);

        /// <summary>
        /// 更新文档
        /// </summary>
        Task<bool> UpdateAsync(string id, T docuemnt);

        /// <summary>
        /// 更新文档
        /// </summary>
        int BulkUpdate(Func<T, string> idAction, params T[] documents);

        /// <summary>
        /// 更新文档
        /// </summary>
        Task<int> BulkUpdateAsync(Func<T, string> idAction, params T[] documents);

        /// <summary>
        /// 更新文档
        /// </summary>
        bool Upsert(string id, T docuemnt);

        /// <summary>
        /// 更新文档
        /// </summary>
        Task<bool> UpsertAsync(string id, T docuemnt);

        /// <summary>
        /// 更新文档
        /// </summary>
        int BulkUpsert(Func<T, string> idAction, params T[] documents);

        /// <summary>
        /// 更新文档
        /// </summary>
        Task<int> BulkUpsertAsync(Func<T, string> idAction, params T[] documents);

        /// <summary>
        /// 更新文档
        /// </summary>
        bool Update<TPartial>(string id, TPartial docuemnt) where TPartial : class, new();

        /// <summary>
        /// 更新文档
        /// </summary>
        Task<bool> UpdateAsync<TPartial>(string id, TPartial docuemnt) where TPartial : class, new();

        /// <summary>
        /// 更新文档
        /// </summary>
        int BulkUpdate<TPartial>(Func<TPartial, string> idAction, params TPartial[] documents) where TPartial : class, new();

        /// <summary>
        /// 更新文档
        /// </summary>
        Task<int> BulkUpdateAsync<TPartial>(Func<TPartial, string> idAction, params TPartial[] documents) where TPartial : class, new();

        /// <summary>
        /// 文档计数
        /// </summary>
        long Count(Func<QueryContainerDescriptor<T>, QueryContainer> query = null);

        /// <summary>
        /// 获取文档
        /// </summary>
        T Get(string id);

        /// <summary>
        /// 获取文档
        /// </summary>
        Task<T> GetAsync(string id);

        /// <summary>
        /// 查询文档
        /// </summary>
        (IEnumerable<T>, long?) Search(Func<QueryContainerDescriptor<T>, QueryContainer> query, Func<SortDescriptor<T>, IPromise<IList<ISort>>> sort = null, int? from = null, int? size = null);

        /// <summary>
        /// 查询文档
        /// </summary>
        Task<(IEnumerable<T>, long?)> SearchAsync(Func<QueryContainerDescriptor<T>, QueryContainer> query, Func<SortDescriptor<T>, IPromise<IList<ISort>>> sort = null, int? from = null, int? size = null);

    }
}
