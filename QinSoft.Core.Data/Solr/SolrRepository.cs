using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using QinSoft.Core.Common.Utils;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace QinSoft.Core.Data.Solr
{
    /// <summary>
    /// Solr仓库基类
    /// </summary>
    public class SolrRepository<T> : ISolrRepository<T> where T : class, new()
    {
        /// <summary>
        /// 当前上下文Solr客户端
        /// </summary>
        public virtual ISolrOperations<T> Client
        {
            get
            {
                Type type = typeof(T);
                SolrCoreAttribute attribute = type.GetCustomAttributes(typeof(SolrCoreAttribute), false).FirstOrDefault() as SolrCoreAttribute;
                if (attribute != null && !attribute.Name.IsEmpty())
                {
                    return SolrContext.Instance.Get<T>(attribute.Name, CoreName);
                }
                return SolrContext.Instance.Get<T>(CoreName);
            }
        }

        /// <summary>
        /// 核心名
        /// </summary>
        public virtual string CoreName
        {
            get
            {
                Type type = typeof(T);
                SolrCoreAttribute attribute = type.GetCustomAttributes(typeof(SolrCoreAttribute), false).FirstOrDefault() as SolrCoreAttribute;
                if (attribute != null)
                {
                    return attribute.CoreName;
                }
                return type.Name;
            }
        }

        /// <summary>
        /// 保存文档
        /// </summary>
        public virtual bool Add(T document, AddParameters parameters = null)
        {
            ResponseHeader response = parameters!=null? this.Client.Add(document, parameters):this.Client.Add(document);
            return response.Status.Equals(0);
        }

        /// <summary>
        /// 保存文档
        /// </summary>
        public virtual bool AddRange(T[] documents, AddParameters parameters = null)
        {
            ResponseHeader response = parameters != null ? this.Client.AddRange(documents, parameters) : this.Client.AddRange(documents);
            return response.Status.Equals(0);
        }

        /// <summary>
        /// 保存文档
        /// </summary>
        public virtual async Task<bool> AddAsync(T document, AddParameters parameters=null)
        {
            ResponseHeader response = parameters != null ? await this.Client.AddAsync(document, parameters): await this.Client.AddAsync(document);
            return response.Status.Equals(0);
        }

        /// <summary>
        /// 保存文档
        /// </summary>
        public virtual async Task<bool> AddRangeAsync(T[] documents, AddParameters parameters=null)
        {
            ResponseHeader response = parameters != null ? await this.Client.AddRangeAsync(documents, parameters): await this.Client.AddRangeAsync(documents);
            return response.Status.Equals(0);
        }

        /// <summary>
        /// 移除文档
        /// </summary>
        public virtual bool Delete(T document,DeleteParameters parameters=null)
        {
            ResponseHeader response = parameters!=null? this.Client.Delete(document,parameters):this.Client.Delete(document);
            return response.Status.Equals(0);
        }

        /// <summary>
        /// 移除文档
        /// </summary>
        public virtual bool Delete(T[] documents, DeleteParameters parameters = null)
        {
            ResponseHeader response = parameters != null ? this.Client.Delete(documents, parameters): this.Client.Delete(documents);
            return response.Status.Equals(0);
        }

        /// <summary>
        /// 移除文档
        /// </summary>
        public virtual bool Delete(string id, DeleteParameters parameters = null)
        {
            ResponseHeader response = parameters != null ? this.Client.Delete(id,parameters): this.Client.Delete(id);
            return response.Status.Equals(0);
        }

        /// <summary>
        /// 移除文档
        /// </summary>
        public virtual bool Delete(string[] ids, DeleteParameters parameters = null)
        {
            ResponseHeader response = parameters != null ? this.Client.Delete(ids,parameters): this.Client.Delete(ids);
            return response.Status.Equals(0);
        }

        /// <summary>
        /// 移除文档
        /// </summary>
        public virtual bool Delete(ISolrQuery query, DeleteParameters parameters = null)
        {
            ResponseHeader response = parameters != null ? this.Client.Delete(query,parameters): this.Client.Delete(query);
            return response.Status.Equals(0);
        }

        /// <summary>
        /// 移除文档
        /// </summary>
        public virtual async Task<bool> DeleteAsync(T document, DeleteParameters parameters = null)
        {
            ResponseHeader response = parameters != null ? await this.Client.DeleteAsync(document, parameters) : await this.Client.DeleteAsync(document);
            return response.Status.Equals(0);
        }

        /// <summary>
        /// 移除文档
        /// </summary>
        public virtual async Task<bool> DeleteAsync(T[] documents, DeleteParameters parameters = null)
        {
            ResponseHeader response = parameters != null ? await this.Client.DeleteAsync(documents, parameters) : await this.Client.DeleteAsync(documents);
            return response.Status.Equals(0);
        }

        /// <summary>
        /// 移除文档
        /// </summary>
        public virtual async Task<bool> DeleteAsync(string id, DeleteParameters parameters = null)
        {
            ResponseHeader response = parameters != null ? await this.Client.DeleteAsync(id, parameters) : await this.Client.DeleteAsync(id);
            return response.Status.Equals(0);
        }

        /// <summary>
        /// 移除文档
        /// </summary>
        public virtual async Task<bool> DeleteAsync(string[] ids, DeleteParameters parameters = null)
        {
            ResponseHeader response = parameters != null ? await this.Client.DeleteAsync(ids, parameters) : await this.Client.DeleteAsync(ids);
            return response.Status.Equals(0);
        }

        /// <summary>
        /// 移除文档
        /// </summary>
        public virtual async Task<bool> DeleteAsync(ISolrQuery query, DeleteParameters parameters = null)
        {
            ResponseHeader response = parameters != null ? await this.Client.DeleteAsync(query, parameters) : await this.Client.DeleteAsync(query);
            return response.Status.Equals(0);
        }

        /// <summary>
        /// 提交操作
        /// </summary>
        public virtual bool Commit()
        {
            ResponseHeader response = this.Client.Commit();
            return response.Status.Equals(0);
        }

        /// <summary>
        /// 提交操作
        /// </summary>
        public virtual async Task<bool> CommitAsync()
        {
            ResponseHeader response = await this.Client.CommitAsync();
            return response.Status.Equals(0);
        }

        /// <summary>
        /// 查询文档
        /// </summary>
        public virtual IEnumerable<T> Query(string query, QueryOptions options=null)
        {
            SolrQueryResults<T> response = options!=null? this.Client.Query(query, options): this.Client.Query(query);
            return response;
        }

        /// <summary>
        /// 查询文档
        /// </summary>
        public virtual IEnumerable<T> Query(ISolrQuery query, QueryOptions options = null)
        {
            SolrQueryResults<T> response = options != null ? this.Client.Query(query, options) : this.Client.Query(query);
            return response;
        }

        /// <summary>
        /// 查询文档
        /// </summary>
        public virtual IEnumerable<T> Query(string query, SortOrder[] sorts = null)
        {
            SolrQueryResults<T> response = !sorts.IsEmpty()? this.Client.Query(query,sorts): this.Client.Query(query);
            return response;
        }

        /// <summary>
        /// 查询文档
        /// </summary>
        public virtual IEnumerable<T> Query(ISolrQuery query, SortOrder[] sorts = null)
        {
            SolrQueryResults<T> response = !sorts.IsEmpty() ? this.Client.Query(query, sorts) : this.Client.Query(query);
            return response;
        }

        /// <summary>
        /// 查询文档
        /// </summary>
        public virtual async Task<IEnumerable<T>> QueryAsync(string query, QueryOptions options = null, CancellationToken cancellationToken = default)
        {
            SolrQueryResults<T> response = options != null ? await this.Client.QueryAsync(query, options, cancellationToken) : await this.Client.QueryAsync(query, cancellationToken);
            return response;
        }

        /// <summary>
        /// 查询文档
        /// </summary>
        public virtual async Task<IEnumerable<T>> QueryAsync(ISolrQuery query, QueryOptions options = null, CancellationToken cancellationToken = default)
        {
            SolrQueryResults<T> response = options != null ? await this.Client.QueryAsync(query, options, cancellationToken) : await this.Client.QueryAsync(query, cancellationToken);
            return response;
        }

        /// <summary>
        /// 查询文档
        /// </summary>
        public virtual async Task<IEnumerable<T>> QueryAsync(string query, SortOrder[] sorts = null, CancellationToken cancellationToken = default)
        {
            SolrQueryResults<T> response = !sorts.IsEmpty() ? await this.Client.QueryAsync(query, sorts, cancellationToken) : await this.Client.QueryAsync(query, cancellationToken);
            return response;
        }

        /// <summary>
        /// 查询文档
        /// </summary>
        public virtual async Task<IEnumerable<T>> QueryAsync(ISolrQuery query, SortOrder[] sorts = null,CancellationToken cancellationToken=default)
        {
            SolrQueryResults<T> response = !sorts.IsEmpty() ? await this.Client.QueryAsync(query, sorts, cancellationToken) : await this.Client.QueryAsync(query, cancellationToken);
            return response;
        }
    }
}
