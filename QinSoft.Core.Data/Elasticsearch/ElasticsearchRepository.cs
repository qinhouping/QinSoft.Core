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
                if (attribute != null && !string.IsNullOrEmpty(attribute.Name))
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
            IndexResponse response = Client.Index(document, i => i.Index(IndexName));
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
            IndexResponse response = await Client.IndexAsync(document, (i) => i.Index(IndexName));
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
        /// 移除文档
        /// </summary>
        public virtual bool Delete(string id)
        {
            DeleteResponse response = Client.Delete(new DocumentPath<T>(id), s => s.Index(IndexName));
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
            DeleteResponse response = await Client.DeleteAsync(new DocumentPath<T>(new Id(id)), s => s.Index(IndexName));
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
        /// 更新文档
        /// </summary>
        public virtual bool Update(string id, T docuemnt)
        {
            UpdateResponse<T> response = Client.Update(new DocumentPath<T>(id), s => s.Index(IndexName).Doc(docuemnt));
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
            UpdateResponse<T> response = await Client.UpdateAsync(new DocumentPath<T>(id), s => s.Index(IndexName).Doc(docuemnt));
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
        public virtual bool Update<TPartial>(string id, TPartial docuemnt) where TPartial : class, new()
        {
            UpdateResponse<T> response = Client.Update<T, TPartial>(new DocumentPath<T>(id), s => s.Index(IndexName).Doc(docuemnt));
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
            UpdateResponse<T> response = await Client.UpdateAsync<T, TPartial>(new DocumentPath<T>(id), s => s.Index(IndexName).Doc(document));
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
        /// 获取文档
        /// </summary>
        public virtual T Get(string id)
        {
            GetResponse<T> response = Client.Get(new DocumentPath<T>(id), s => s.Index(IndexName));
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
            GetResponse<T> response = await Client.GetAsync(new DocumentPath<T>(id), s => s.Index(IndexName));
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
            ISearchResponse<T> response = Client.Search<T>(s => s.Index(IndexName).Sort(sort).From(from).Size(size).Query(query));
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
            ISearchResponse<T> response = await Client.SearchAsync<T>(s => s.Index(IndexName).Sort(sort).From(from).Size(size).Query(query));
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
