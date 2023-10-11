using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using QinSoft.Core.Common.Utils;
using System.Threading.Tasks;

namespace QinSoft.Core.Data.Database
{
    /// <summary>
    /// 数据库仓库基类
    /// </summary>
    public class DatabaseRepository<T> : IDatabaseRepository<T> where T : class, new()
    {
        /// <summary>
        /// 当前上下文数据库客户端
        /// </summary>
        public virtual ISqlSugarClient Client
        {
            get
            {
                return DatabaseContextStack.Instance.Peek().AsTenant().GetConnectionWithAttr<T>();
            }
        }

        /// <summary>
        /// 插入实体
        /// </summary>
        public virtual bool Insert(T entity, Action<IInsertable<T>> action = null)
        {
            IInsertable<T> insertable = Client.Insertable<T>(entity);
            if (action != null)
            {
                action(insertable);
            }
            return insertable.ExecuteCommand() > 0;
        }

        /// <summary>
        /// 插入实体
        /// </summary>
        public virtual async Task<bool> InsertAsync(T entity, Action<IInsertable<T>> action = null)
        {
            IInsertable<T> insertable = Client.Insertable<T>(entity);
            if (action != null)
            {
                action(insertable);
            }
            return await insertable.ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 批量插入实体
        /// </summary>
        public virtual int Insert(IEnumerable<T> entities, Action<IInsertable<T>> action = null)
        {
            IInsertable<T> insertable = Client.Insertable<T>(new List<T>(entities));
            if (action != null)
            {
                action(insertable);
            }
            return insertable.ExecuteCommand();
        }


        /// <summary>
        /// 批量插入实体
        /// </summary>
        public virtual async Task<int> InsertAsync(IEnumerable<T> entities, Action<IInsertable<T>> action = null)
        {
            IInsertable<T> insertable = Client.Insertable<T>(new List<T>(entities));
            if (action != null)
            {
                action(insertable);
            }
            return await insertable.ExecuteCommandAsync();
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        public virtual int Update(Action<IUpdateable<T>> action)
        {
            IUpdateable<T> updateable = Client.Updateable<T>();
            if (action != null)
            {
                action(updateable);
            }
            return updateable.ExecuteCommand();
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        public virtual async Task<int> UpdateAsync(Action<IUpdateable<T>> action)
        {
            IUpdateable<T> updateable = Client.Updateable<T>();
            if (action != null)
            {
                action(updateable);
            }
            return await updateable.ExecuteCommandAsync();
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        public virtual bool Update(T entity, Action<IUpdateable<T>> action = null)
        {
            IUpdateable<T> updateable = Client.Updateable<T>(entity);
            if (action != null)
            {
                action(updateable);
            }
            return updateable.ExecuteCommand() > 0;
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        public virtual async Task<bool> UpdateAsync(T entity, Action<IUpdateable<T>> action = null)
        {
            IUpdateable<T> updateable = Client.Updateable<T>(entity);
            if (action != null)
            {
                action(updateable);
            }
            return await updateable.ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 批量更新实体
        /// </summary>
        public virtual int Update(IEnumerable<T> entities, Action<IUpdateable<T>> action = null)
        {
            IUpdateable<T> updateable = Client.Updateable<T>(new List<T>(entities));
            if (action != null)
            {
                action(updateable);
            }
            return updateable.ExecuteCommand();
        }

        /// <summary>
        /// 批量更新实体
        /// </summary>
        public virtual async Task<int> UpdateAsync(IEnumerable<T> entities, Action<IUpdateable<T>> action = null)
        {
            IUpdateable<T> updateable = Client.Updateable<T>(new List<T>(entities));
            if (action != null)
            {
                action(updateable);
            }
            return await updateable.ExecuteCommandAsync();
        }

        /// <summary>
        /// 保存实体
        /// </summary>
        public virtual bool Save(T entity, Action<IStorageable<T>> action = null)
        {
            IStorageable<T> saveable = Client.Storageable<T>(entity);
            if (action != null)
            {
                action(saveable);
            }
            return saveable.ExecuteCommand() > 0;
        }

        /// <summary>
        /// 保存实体
        /// </summary>
        public virtual async Task<bool> SaveAsync(T entity, Action<IStorageable<T>> action = null)
        {
            IStorageable<T> saveable = Client.Storageable<T>(entity);
            if (action != null)
            {
                action(saveable);
            }
            return await saveable.ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 批量保存实体
        /// </summary>
        public virtual int Save(IEnumerable<T> entities, Action<IStorageable<T>> action = null)
        {
            IStorageable<T> saveable = Client.Storageable<T>(new List<T>(entities));
            if (action != null)
            {
                action(saveable);
            }
            return saveable.ExecuteCommand();
        }


        /// <summary>
        /// 批量保存实体
        /// </summary>
        public virtual async Task<int> SaveAsync(IEnumerable<T> entities, Action<IStorageable<T>> action = null)
        {
            IStorageable<T> saveable = Client.Storageable<T>(new List<T>(entities));
            if (action != null)
            {
                action(saveable);
            }
            return await saveable.ExecuteCommandAsync();
        }

        /// <summary>
        /// 移除实体
        /// </summary>
        public virtual int Delete(Action<IDeleteable<T>> action)
        {
            IDeleteable<T> deleteable = Client.Deleteable<T>();
            if (action != null)
            {
                action(deleteable);
            }
            return deleteable.ExecuteCommand();
        }

        /// <summary>
        /// 移除实体
        /// </summary>
        public virtual async Task<int> DeleteAsync(Action<IDeleteable<T>> action)
        {
            IDeleteable<T> deleteable = Client.Deleteable<T>();
            if (action != null)
            {
                action(deleteable);
            }
            return await deleteable.ExecuteCommandAsync();
        }

        /// <summary>
        /// 移除实体
        /// </summary>
        public virtual bool Delete(params object[] keys)
        {
            return Client.Deleteable<T>(keys).ExecuteCommand() > 0;
        }

        /// <summary>
        /// 移除实体
        /// </summary>
        public virtual async Task<bool> DeleteAsync(params object[] keys)
        {
            return await Client.Deleteable<T>(keys).ExecuteCommandAsync() > 0;
        }


        /// <summary>
        /// 移除实体
        /// </summary>
        public virtual bool Delete(T entity, Action<IDeleteable<T>> action = null)
        {
            IDeleteable<T> deleteable = Client.Deleteable<T>(entity);
            if (action != null)
            {
                action(deleteable);
            }
            return deleteable.ExecuteCommand() > 0;
        }

        /// <summary>
        /// 移除实体
        /// </summary>
        public virtual async Task<bool> DeleteAsync(T entity, Action<IDeleteable<T>> action = null)
        {
            IDeleteable<T> deleteable = Client.Deleteable<T>(entity);
            if (action != null)
            {
                action(deleteable);
            }
            return await deleteable.ExecuteCommandAsync() > 0;
        }

        /// <summary>
        /// 移除实体
        /// </summary>
        public virtual int Delete(IEnumerable<T> entities, Action<IDeleteable<T>> action = null)
        {
            IDeleteable<T> deleteable = Client.Deleteable<T>(entities);
            if (action != null)
            {
                action(deleteable);
            }
            return deleteable.ExecuteCommand();
        }

        /// <summary>
        /// 移除实体
        /// </summary>
        public virtual async Task<int> DeleteAsync(IEnumerable<T> entities, Action<IDeleteable<T>> action = null)
        {
            IDeleteable<T> deleteable = Client.Deleteable<T>(entities);
            if (action != null)
            {
                action(deleteable);
            }
            return await deleteable.ExecuteCommandAsync();
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        public virtual IEnumerable<T> Select(Action<ISugarQueryable<T>> action = null)
        {
            ISugarQueryable<T> queryable = Client.Queryable<T>();
            if (action != null)
            {
                action(queryable);
            }
            return queryable.ToList();
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        public virtual async Task<IEnumerable<T>> SelectAsync(Action<ISugarQueryable<T>> action = null)
        {
            ISugarQueryable<T> queryable = Client.Queryable<T>();
            if (action != null)
            {
                action(queryable);
            }
            return await queryable.ToListAsync();
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        public virtual T SelectOne(params object[] keys)
        {
            return Client.Queryable<T>().In(keys).First();
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        public virtual async Task<T> SelectOneAsync(params object[] keys)
        {
            return await Client.Queryable<T>().In(keys).FirstAsync();
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        public virtual T SelectOne(Action<ISugarQueryable<T>> action)
        {
            ISugarQueryable<T> queryable = Client.Queryable<T>();
            if (action != null)
            {
                action(queryable);
            }
            return queryable.First();
        }


        /// <summary>
        /// 查询实体
        /// </summary>
        public virtual async Task<T> SelectOneAsync(Action<ISugarQueryable<T>> action)
        {
            ISugarQueryable<T> queryable = Client.Queryable<T>();
            if (action != null)
            {
                action(queryable);
            }
            return await queryable.FirstAsync();
        }


        /// <summary>
        /// 计数
        /// </summary>
        public virtual long Count(Action<ISugarQueryable<T>> action = null)
        {
            ISugarQueryable<T> queryable = Client.Queryable<T>();
            if (action != null)
            {
                action(queryable);
            }
            return queryable.Count();
        }


        /// <summary>
        /// 计数
        /// </summary>
        public virtual async Task<long> CountAsync(Action<ISugarQueryable<T>> action = null)
        {
            ISugarQueryable<T> queryable = Client.Queryable<T>();
            if (action != null)
            {
                action(queryable);
            }
            return await queryable.CountAsync();
        }
    }
}
