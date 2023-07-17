using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Reflection;
using MongoDB.Bson.Serialization.Attributes;

namespace QinSoft.Core.Data.MongoDB.Core
{
    /// <summary>
    /// mongodb仓库基类
    /// </summary>
    public class MongoDBRepository<T> : IMongoDBRepository<T> where T : class, new()
    {
        /// <summary>
        /// 当前上下文数据库
        /// </summary>
        public virtual IMongoDatabase Database
        {
            get
            {
                Type type = typeof(T);
                MongoDBCollectionAttribute attribute = type.GetCustomAttributes(typeof(MongoDBCollectionAttribute), false).FirstOrDefault() as MongoDBCollectionAttribute;
                if (attribute != null && !string.IsNullOrEmpty(attribute.Name))
                {
                    return MongoDBContext.Instance.Get(attribute.Name).GetDatabase();
                }
                return MongoDBContext.Instance.Get().GetDatabase();
            }
        }

        /// <summary>
        /// 当前上下文集合
        /// </summary>
        public virtual IMongoCollection<T> Collection
        {
            get
            {
                Type type = typeof(T);
                MongoDBCollectionAttribute attribute = type.GetCustomAttributes(typeof(MongoDBCollectionAttribute), false).FirstOrDefault() as MongoDBCollectionAttribute;
                if (attribute != null)
                {
                    return Database.GetCollection<T>(attribute.CollectionName);
                }
                return Database.GetCollection<T>(type.Name);
            }
        }

        /// <summary>
        /// 创建过滤定义
        /// </summary>
        public virtual FilterDefinition<T> CreateFilterDefinition(IDictionary<string, object> keyValues)
        {
            FilterDefinitionBuilder<T> builder = Builders<T>.Filter;
            FilterDefinition<T> filter = null;
            ParameterExpression parameter = Expression.Parameter(typeof(T), "t");
            foreach (KeyValuePair<string, object> keyValue in keyValues)
            {
                MemberExpression member = Expression.PropertyOrField(parameter, keyValue.Key);

                if (filter == null) filter = builder.Eq(Expression.Lambda<Func<T, object>>(member, parameter), keyValue.Value);
                else filter &= builder.Eq(Expression.Lambda<Func<T, object>>(member, parameter), keyValue.Value);
            }
            return filter;
        }

        /// <summary>
        /// 创建过滤定义
        /// </summary>
        public virtual FilterDefinition<T> CreateFilterDefinition(T document)
        {
            Dictionary<string, object> keyValues = new Dictionary<string, object>();
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                BsonIdAttribute attribute = propertyInfo.GetCustomAttribute(typeof(BsonIdAttribute)) as BsonIdAttribute;
                if (attribute != null)
                    keyValues.Add(propertyInfo.Name, propertyInfo.GetValue(document));
            }
            return CreateFilterDefinition(keyValues);
        }

        /// <summary>
        /// 创建更新定义
        /// </summary>
        public virtual UpdateDefinition<T> CreateUpdateDefinition(IDictionary<string, object> keyValues)
        {
            UpdateDefinitionBuilder<T> builder = Builders<T>.Update;
            UpdateDefinition<T> update = null;
            ParameterExpression parameter = Expression.Parameter(typeof(T), "t");
            foreach (KeyValuePair<string, object> keyValue in keyValues)
            {
                MemberExpression member = Expression.PropertyOrField(parameter, keyValue.Key);
                if (update == null) update = builder.Set(Expression.Lambda<Func<T, object>>(member, parameter), keyValue.Value);
                else update = builder.Combine(update, builder.Set(Expression.Lambda<Func<T, object>>(member, parameter), keyValue.Value));
            }
            return update;
        }

        /// <summary>
        /// 创建更新定义
        /// </summary>
        public virtual UpdateDefinition<T> CreateUpdateDefinition(T document)
        {
            Dictionary<string, object> keyValues = new Dictionary<string, object>();
            PropertyInfo[] propertyInfos = typeof(T).GetProperties();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                keyValues.Add(propertyInfo.Name, propertyInfo.GetValue(document));
            }
            return CreateUpdateDefinition(keyValues);
        }

        /// <summary>
        /// 插入文档
        /// </summary>
        public virtual bool InsertOne(T document, InsertOneOptions options = null, CancellationToken cancellationToken = default)
        {
            Collection.InsertOne(document, options, cancellationToken);
            return true;
        }

        /// <summary>
        /// 插入文档
        /// </summary>
        public virtual async Task<bool> InsertOneAsync(T document, InsertOneOptions options = null, CancellationToken cancellationToken = default)
        {
            await Collection.InsertOneAsync(document, options, cancellationToken);
            return true;
        }

        /// <summary>
        /// 批量插入文档
        /// </summary>
        public virtual bool InsertMany(IEnumerable<T> documents, InsertManyOptions options = null, CancellationToken cancellationToken = default)
        {
            Collection.InsertMany(documents, options, cancellationToken);
            return true;
        }

        /// <summary>
        /// 批量插入文档
        /// </summary>
        public virtual async Task<bool> InsertManyAsync(IEnumerable<T> documents, InsertManyOptions options = null, CancellationToken cancellationToken = default)
        {
            await Collection.InsertManyAsync(documents, options, cancellationToken);
            return true;
        }

        /// <summary>
        /// 更新文档
        /// </summary>
        public virtual bool UpdateOne(FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            return Collection.UpdateOne(filter, update, options, cancellationToken).ModifiedCount > 0;
        }

        /// <summary>
        /// 更新文档
        /// </summary>
        public virtual async Task<bool> UpdateOneAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            return (await Collection.UpdateOneAsync(filter, update, options, cancellationToken)).ModifiedCount > 0;
        }

        /// <summary>
        /// 更新文档
        /// </summary>
        public virtual bool UpdateMany(FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            return Collection.UpdateMany(filter, update, options, cancellationToken).ModifiedCount > 0;
        }

        /// <summary>
        /// 更新文档
        /// </summary>
        public virtual async Task<bool> UpdateManyAsync(FilterDefinition<T> filter, UpdateDefinition<T> update, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            return (await Collection.UpdateManyAsync(filter, update, options, cancellationToken)).ModifiedCount > 0;
        }

        /// <summary>
        /// 更新文档
        /// </summary>
        public virtual bool UpdateOne(T document, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            return Collection.UpdateOne(CreateFilterDefinition(document), CreateUpdateDefinition(document), options, cancellationToken).ModifiedCount > 0;
        }

        /// <summary>
        /// 更新文档
        /// </summary>
        public virtual async Task<bool> UpdateOneAsync(T document, UpdateOptions options = null, CancellationToken cancellationToken = default)
        {
            return (await Collection.UpdateOneAsync(CreateFilterDefinition(document), CreateUpdateDefinition(document), options, cancellationToken)).ModifiedCount > 0;
        }

        /// <summary>
        /// 移除文档
        /// </summary>
        public virtual bool DeleteOne(FilterDefinition<T> filter, CancellationToken cancellationToken = default)
        {
            return Collection.DeleteOne(filter, cancellationToken).DeletedCount > 0;
        }

        /// <summary>
        /// 移除文档
        /// </summary>
        public virtual async Task<bool> DeleteOneAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default)
        {
            return (await Collection.DeleteOneAsync(filter, cancellationToken)).DeletedCount > 0;
        }

        /// <summary>
        /// 移除文档
        /// </summary>
        public virtual bool DeleteOne(T docuemnt, CancellationToken cancellationToken = default)
        {
            return Collection.DeleteOne(CreateFilterDefinition(docuemnt), cancellationToken).DeletedCount > 0;
        }

        /// <summary>
        /// 移除文档
        /// </summary>
        public virtual async Task<bool> DeleteOneAsync(T docuemnt, CancellationToken cancellationToken = default)
        {
            return (await Collection.DeleteOneAsync(CreateFilterDefinition(docuemnt), cancellationToken)).DeletedCount > 0;
        }

        /// <summary>
        /// 移除文档
        /// </summary>
        public virtual bool DeleteMany(FilterDefinition<T> filter, CancellationToken cancellationToken = default)
        {
            return Collection.DeleteMany(filter, cancellationToken).DeletedCount > 0;
        }

        /// <summary>
        /// 移除文档
        /// </summary>
        public virtual async Task<bool> DeleteManyAsync(FilterDefinition<T> filter, CancellationToken cancellationToken = default)
        {
            return (await Collection.DeleteManyAsync(filter, cancellationToken)).DeletedCount > 0;
        }

        /// <summary>
        /// 查询文档
        /// </summary>
        public virtual IEnumerable<T> Find(FilterDefinition<T> filter, SortDefinition<T> sort = null, int? skip = null, int? limit = 0)
        {
            IFindFluent<T, T> find = Collection.Find(filter);
            if (sort != null)
                find = find.Sort(sort);
            if (skip != null)
                find = find.Skip(skip);
            if (limit != null)
                find = find.Limit(limit);
            return find.ToList();
        }

        /// <summary>
        /// 查询文档
        /// </summary>
        public virtual async Task<IEnumerable<T>> FindAsync(FilterDefinition<T> filter, SortDefinition<T> sort = null, int? skip = null, int? limit = 0)
        {
            IFindFluent<T, T> find = Collection.Find(filter);
            if (sort != null)
                find = find.Sort(sort);
            if (skip != null)
                find = find.Skip(skip);
            if (limit != null)
                find = find.Limit(limit);
            return await find.ToListAsync();
        }

        /// <summary>
        /// 查询文档
        /// </summary>
        public virtual T FindOne(FilterDefinition<T> filter)
        {
            IFindFluent<T, T> find = Collection.Find(filter);
            return find.FirstOrDefault();
        }

        /// <summary>
        /// 查询文档
        /// </summary>
        public virtual async Task<T> FindOneAsync(FilterDefinition<T> filter)
        {
            IFindFluent<T, T> find = Collection.Find(filter);
            return await find.FirstOrDefaultAsync();
        }

        /// <summary>
        /// 查询文档
        /// </summary>
        public virtual T FindOne(T docuemnt)
        {
            IFindFluent<T, T> find = Collection.Find(CreateFilterDefinition(docuemnt));
            return find.FirstOrDefault();
        }

        /// <summary>
        /// 查询文档
        /// </summary>
        public virtual async Task<T> FindOneAsync(T docuemnt)
        {
            IFindFluent<T, T> find = Collection.Find(CreateFilterDefinition(docuemnt));
            return await find.FirstOrDefaultAsync();
        }


        /// <summary>
        /// 批量操作文档
        /// </summary>
        public virtual long BulkWrite(IEnumerable<WriteModel<T>> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default)
        {
            return Collection.BulkWrite(requests, options, cancellationToken).ModifiedCount;
        }

        /// <summary>
        /// 批量操作文档
        /// </summary>
        public virtual async Task<long> BulkWriteAsync(IEnumerable<WriteModel<T>> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default)
        {
            return (await Collection.BulkWriteAsync(requests, options, cancellationToken)).ModifiedCount;
        }

        /// <summary>
        /// 批量插入文档
        /// </summary>
        public virtual long BulkInsert(IEnumerable<T> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default)
        {
            List<WriteModel<T>> req = new List<WriteModel<T>>();
            foreach (T t in requests)
            {
                req.Add(new InsertOneModel<T>(t));
            }
            return Collection.BulkWrite(req, options, cancellationToken).ModifiedCount;
        }

        /// <summary>
        /// 批量插入文档
        /// </summary>
        public virtual async Task<long> BulkInsertAsync(IEnumerable<T> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default)
        {
            List<WriteModel<T>> req = new List<WriteModel<T>>();
            foreach (T t in requests)
            {
                req.Add(new InsertOneModel<T>(t));
            }
            return (await Collection.BulkWriteAsync(req, options, cancellationToken)).ModifiedCount;
        }

        /// <summary>
        /// 批量插入文档
        /// </summary>
        public virtual long BulkUpdate(IEnumerable<T> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default)
        {
            List<WriteModel<T>> req = new List<WriteModel<T>>();
            foreach (T t in requests)
            {
                req.Add(new UpdateOneModel<T>(CreateFilterDefinition(t), CreateUpdateDefinition(t)));
            }
            return Collection.BulkWrite(req, options, cancellationToken).ModifiedCount;
        }

        /// <summary>
        /// 批量插入文档
        /// </summary>
        public virtual async Task<long> BulkUpdateAsync(IEnumerable<T> requests, BulkWriteOptions options = null, CancellationToken cancellationToken = default)
        {
            List<WriteModel<T>> req = new List<WriteModel<T>>();
            foreach (T t in requests)
            {
                req.Add(new UpdateOneModel<T>(CreateFilterDefinition(t), CreateUpdateDefinition(t)));
            }
            return (await Collection.BulkWriteAsync(req, options, cancellationToken)).ModifiedCount;
        }
    }
}
