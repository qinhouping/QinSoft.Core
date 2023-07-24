using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QinSoft.Core.Data.MongoDB.Core
{
    /// <summary>
    /// mongodb仓库接口
    /// </summary>
    public interface IMongoDBRepository<T> where T : class, new()
    {
        /// <summary>
        /// 当前上下文数据库
        /// </summary>
        IMongoDatabase Database { get; }

        /// <summary>
        /// 创建过滤定义
        /// </summary>
        FilterDefinition<T> CreateFilterDefinition(IDictionary<string, object> keyValues);

        /// <summary>
        /// 创建过滤定义
        /// </summary>
        FilterDefinition<T> CreateFilterDefinition(T document);

        /// <summary>
        /// 创建更新定义
        /// </summary>
        UpdateDefinition<T> CreateUpdateDefinition(IDictionary<string, object> keyValues);

        /// <summary>
        /// 创建更新定义
        /// </summary>
        UpdateDefinition<T> CreateUpdateDefinition(T document);

        /// <summary>
        /// 插入文档
        /// </summary>
        bool InsertOne(T document, InsertOneOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 插入文档
        /// </summary>
        Task<bool> InsertOneAsync(T document, InsertOneOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量插入文档
        /// </summary>
        bool InsertMany(IEnumerable<T> documents, InsertManyOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量插入文档
        /// </summary>
        Task<bool> InsertManyAsync(IEnumerable<T> documents, InsertManyOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新文档
        /// </summary>
        bool UpdateOne(FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新文档
        /// </summary>
        Task<bool> UpdateOneAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新文档
        /// </summary>
        bool UpdateMany(FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null, CancellationToken cancellationToken = default);
        /// <summary>
        /// 更新文档
        /// </summary>
        Task<bool> UpdateManyAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新文档
        /// </summary>
        bool UpdateOne(T document, UpdateOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 更新文档
        /// </summary>
        Task<bool> UpdateOneAsync(T document, UpdateOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 移除文档
        /// </summary>
        bool DeleteOne(FilterDefinition<T> filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// 移除文档
        /// </summary>
        Task<bool> DeleteOneAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// 移除文档
        /// </summary>
        bool DeleteOne(T docuemnt, CancellationToken cancellationToken = default);

        /// <summary>
        /// 移除文档
        /// </summary>
        Task<bool> DeleteOneAsync(T docuemnt, CancellationToken cancellationToken = default);

        /// <summary>
        /// 移除文档
        /// </summary>
        bool DeleteMany(FilterDefinition<T> filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// 移除文档
        /// </summary>
        Task<bool> DeleteManyAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// 查询文档
        /// </summary>
        IEnumerable<T> Find(FilterDefinition<T> filter, SortDefinition<T> sort = null, int? skip = null, int? limit = 0);

        /// <summary>
        /// 查询文档
        /// </summary>
        Task<IEnumerable<T>> FindAsync(FilterDefinition<T> filter, SortDefinition<T> sort = null, int? skip = null, int? limit = 0);

        /// <summary>
        /// 查询文档
        /// </summary>
        T FindOne(FilterDefinition<T> filter);

        /// <summary>
        /// 查询文档
        /// </summary>
        Task<T> FindOneAsync(FilterDefinition<T> filter);

        /// <summary>
        /// 查询文档
        /// </summary>
        T FindOne(T docuemnt);

        /// <summary>
        /// 查询文档
        /// </summary>
        Task<T> FindOneAsync(T docuemnt);

        /// <summary>
        /// 计数文档
        /// </summary>
        long Count(FilterDefinition<T> filter);

        /// <summary>
        /// 计数文档
        /// </summary>
        Task<long> CountAsync(FilterDefinition<T> filter);

        /// <summary>
        /// 批量操作文档
        /// </summary>
        long BulkWrite(IEnumerable<WriteModel<T>> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量操作文档
        /// </summary>
        Task<long> BulkWriteAsync(IEnumerable<WriteModel<T>> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量插入文档
        /// </summary>
        long BulkInsert(IEnumerable<T> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量插入文档
        /// </summary>
        Task<long> BulkInsertAsync(IEnumerable<T> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量插入文档
        /// </summary>
        long BulkUpdate(IEnumerable<T> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 批量插入文档
        /// </summary>
        Task<long> BulkUpdateAsync(IEnumerable<T> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default);
    }
}
