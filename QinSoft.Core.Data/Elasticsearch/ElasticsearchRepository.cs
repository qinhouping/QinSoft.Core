using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using QinSoft.Core.Common.Utils;
using System.Threading.Tasks;
using Nest;
using System.Linq;

namespace QinSoft.Core.Data.Elasticsearch
{
    /// <summary>
    /// elasticsearch仓库基类
    /// </summary>
    public class ElasticsearchRepository<T> : IElasticsearchRepository<T> where T : class, new()
    {
        /// <summary>
        /// 当前上下文elasticsearch客户端
        /// </summary>
        public virtual IElasticClient Client
        {
            get
            {
                Type type = typeof(T);
                ElasticsearchIndexAttribute attribute = type.GetCustomAttributes(typeof(ElasticsearchIndexAttribute), false).FirstOrDefault() as ElasticsearchIndexAttribute;
                if (attribute != null && !attribute.Name.IsEmpty())
                {
                    return ElasticsearchContext.Instance.Get(attribute.Name);
                }
                return ElasticsearchContext.Instance.Get();
            }
        }

        /// <summary>
        /// 索引名
        /// </summary>
        public virtual string IndexName
        {
            get
            {
                Type type = typeof(T);
                ElasticsearchIndexAttribute attribute = type.GetCustomAttributes(typeof(ElasticsearchIndexAttribute), false).FirstOrDefault() as ElasticsearchIndexAttribute;
                if (attribute != null)
                {
                    return attribute.IndexName;
                }
                return type.Name;
            }
        }

        /// <summary>
        /// 索引文档
        /// </summary>
        public virtual (bool, string) Index(T document)
        {
            IndexResponse response = Client.Index<T>(document, i => i.Index(IndexName));
            if (response.IsValid)
            {
                return (Result.Created.Equals(response.Result), response.Id);
            }
            else
            {
                return (false, null);
            }
        }

        /// <summary>
        /// 索引文档
        /// </summary>
        public virtual async Task<(bool, string)> IndexAsync(T document)
        {
            IndexResponse response = await Client.IndexAsync<T>(document, (i) => i.Index(IndexName));
            if (response.IsValid)
            {
                return (Result.Created.Equals(response.Result), response.Id);
            }
            else
            {
                return (false, null);
            }
        }

        /// <summary>
        /// 批量索引文档
        /// </summary>
        public virtual int BulkIndex(params T[] documents)
        {
            BulkResponse response = Client.Bulk(s => s.IndexMany<T>(documents, (i, t) => i.Index(IndexName)));
            if (response.IsValid)
            {
                return response.Items.Count();
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 批量索引文档
        /// </summary>
        public virtual async Task<int> BulkIndexAsync(params T[] documents)
        {
            BulkResponse response = await Client.BulkAsync(s => s.IndexMany<T>(documents, (i, t) => i.Index(IndexName)));
            if (response.IsValid)
            {
                return response.Items.Count();
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 移除文档
        /// </summary>
        public virtual bool Delete(string id)
        {
            DeleteResponse response = Client.Delete<T>(id, s => s.Index(IndexName));
            if (response.IsValid)
            {
                return Result.Deleted.Equals(response.Result);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 移除文档
        /// </summary>
        public virtual async Task<bool> DeleteAsync(string id)
        {
            DeleteResponse response = await Client.DeleteAsync<T>(id, s => s.Index(IndexName));
            if (response.IsValid)
            {
                return Result.Deleted.Equals(response.Result);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 移除文档
        /// </summary>
        public virtual int BulkDelete(params string[] ids)
        {
            BulkResponse response = Client.Bulk(s => s.DeleteMany(ids, (d, t) => d.Index(IndexName)));
            if (response.IsValid)
            {
                return response.Items.Count;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 移除文档
        /// </summary>
        public virtual async Task<int> BulkDeleteAsync(params string[] ids)
        {
            BulkResponse response = await Client.BulkAsync(s => s.DeleteMany(ids, (d, t) => d.Index(IndexName)));
            if (response.IsValid)
            {
                return response.Items.Count;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 更新文档
        /// </summary>
        public virtual bool Update(string id, T docuemnt)
        {
            UpdateResponse<T> response = Client.Update<T>(id, s => s.Index(IndexName).Doc(docuemnt));
            if (response.IsValid)
            {
                return Result.Updated.Equals(response.Result);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新文档
        /// </summary>
        public virtual async Task<bool> UpdateAsync(string id, T docuemnt)
        {
            UpdateResponse<T> response = await Client.UpdateAsync<T>(id, s => s.Index(IndexName).Doc(docuemnt));
            if (response.IsValid)
            {
                return Result.Updated.Equals(response.Result);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新文档
        /// </summary>
        public virtual int BulkUpdate(Func<T, string> idAction, params T[] documents)
        {
            BulkResponse response = Client.Bulk(s => s.UpdateMany<T>(documents, (u, t) => u.Index(IndexName).Id(idAction(t)).Doc(t)));
            if (response.IsValid)
            {
                return response.Items.Count;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 更新文档
        /// </summary>
        public virtual async Task<int> BulkUpdateAsync(Func<T, string> idAction, params T[] documents)
        {
            BulkResponse response = await Client.BulkAsync(s => s.UpdateMany<T>(documents, (u, t) => u.Index(IndexName).Id(idAction(t)).Doc(t)));
            if (response.IsValid)
            {
                return response.Items.Count;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 更新文档
        /// </summary>
        public virtual bool Upsert(string id, T docuemnt)
        {
            UpdateResponse<T> response = Client.Update<T>(id, s => s.Index(IndexName).Upsert(docuemnt));
            if (response.IsValid)
            {
                return Result.Updated.Equals(response.Result);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新文档
        /// </summary>
        public virtual async Task<bool> UpsertAsync(string id, T docuemnt)
        {
            UpdateResponse<T> response = await Client.UpdateAsync<T>(id, s => s.Index(IndexName).Upsert(docuemnt));
            if (response.IsValid)
            {
                return Result.Updated.Equals(response.Result);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新文档
        /// </summary>
        public virtual int BulkUpsert(Func<T, string> idAction, params T[] documents)
        {
            BulkResponse response = Client.Bulk(s => s.UpdateMany<T>(documents, (u, t) => u.Index(IndexName).Id(idAction(t)).Upsert(t)));
            if (response.IsValid)
            {
                return response.Items.Count;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 更新文档
        /// </summary>
        public virtual async Task<int> BulkUpsertAsync(Func<T, string> idAction, params T[] documents)
        {
            BulkResponse response = await Client.BulkAsync(s => s.UpdateMany<T>(documents, (u, t) => u.Index(IndexName).Id(idAction(t)).Upsert(t)));
            if (response.IsValid)
            {
                return response.Items.Count;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 更新文档
        /// </summary>
        public virtual bool Update<TPartial>(string id, TPartial docuemnt) where TPartial : class, new()
        {
            UpdateResponse<T> response = Client.Update<T, TPartial>(id, s => s.Index(IndexName).Doc(docuemnt));
            if (response.IsValid)
            {
                return Result.Updated.Equals(response.Result);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新文档
        /// </summary>
        public virtual async Task<bool> UpdateAsync<TPartial>(string id, TPartial document) where TPartial : class, new()
        {
            UpdateResponse<T> response = await Client.UpdateAsync<T, TPartial>(id, s => s.Index(IndexName).Doc(document));
            if (response.IsValid)
            {
                return Result.Updated.Equals(response.Result);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 更新文档
        /// </summary>
        public virtual int BulkUpdate<TPartial>(Func<TPartial, string> idAction, params TPartial[] documents) where TPartial : class, new()
        {
            BulkResponse response = Client.Bulk(s =>
            {
                foreach (TPartial document in documents)
                {
                    s = s.Update<T, TPartial>(u => u.Index(IndexName).Id(idAction(document)).Doc(document));
                }
                return s;
            });
            if (response.IsValid)
            {
                return response.Items.Count;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 更新文档
        /// </summary>
        public virtual async Task<int> BulkUpdateAsync<TPartial>(Func<TPartial, string> idAction, params TPartial[] documents) where TPartial : class, new()
        {
            BulkResponse response = await Client.BulkAsync(s =>
            {
                foreach (TPartial document in documents)
                {
                    s = s.Update<T, TPartial>(u => u.Index(IndexName).Id(idAction(document)).Doc(document));
                }
                return s;
            });
            if (response.IsValid)
            {
                return response.Items.Count;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 文档计数
        /// </summary>
        public virtual long Count(Func<QueryContainerDescriptor<T>, QueryContainer> query=null)
        {
            CountResponse response = Client.Count<T>( s => s.Index(IndexName).Query(query));
            if (response.IsValid)
            {
                return response.Count;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取文档
        /// </summary>
        public virtual T Get(string id)
        {
            GetResponse<T> response = Client.Get<T>(id, s => s.Index(IndexName));
            if (response.IsValid)
            {
                return response.Source;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 获取文档
        /// </summary>
        public virtual async Task<T> GetAsync(string id)
        {
            GetResponse<T> response = await Client.GetAsync<T>(id, s => s.Index(IndexName));
            if (response.IsValid)
            {
                return response.Source;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 查询文档
        /// </summary>
        public virtual (IEnumerable<T>, long?) Search(Func<QueryContainerDescriptor<T>, QueryContainer> query, Func<SortDescriptor<T>, IPromise<IList<ISort>>> sort = null, int? from = null, int? size = null)
        {
            ISearchResponse<T> response = Client.Search<T>(s => s.Index(IndexName).Query(query).Sort(sort).From(from).Size(size));
            if (response.IsValid)
            {
                return (response.Documents, response.Total);
            }
            else
            {
                return (null, null);
            }
        }

        /// <summary>
        /// 查询文档
        /// </summary>
        public virtual async Task<(IEnumerable<T>, long?)> SearchAsync(Func<QueryContainerDescriptor<T>, QueryContainer> query, Func<SortDescriptor<T>, IPromise<IList<ISort>>> sort = null, int? from = null, int? size = null)
        {
            ISearchResponse<T> response = await Client.SearchAsync<T>(s => s.Index(IndexName).Query(query).Sort(sort).From(from).Size(size));
            if (response.IsValid)
            {
                return (response.Documents, response.Total);
            }
            else
            {
                return (null, null);
            }
        }
    }
}
