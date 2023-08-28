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
    /// 数据库仓库接口
    /// </summary>
    public interface IDatabaseRepository<T> where T : class, new()
    {
        /// <summary>
        /// 当前上下文数据库客户端
        /// </summary>
        ISqlSugarClient Client { get; }

        /// <summary>
        /// 插入实体
        /// </summary>
        bool Insert(T entity, Action<IInsertable<T>> action = null);

        /// <summary>
        /// 插入实体
        /// </summary>
        Task<bool> InsertAsync(T entity, Action<IInsertable<T>> action = null);

        /// <summary>
        /// 批量插入实体
        /// </summary>
        int Insert(IEnumerable<T> entities, Action<IInsertable<T>> action = null);

        /// <summary>
        /// 批量插入实体
        /// </summary>
        Task<int> InsertAsync(IEnumerable<T> entities, Action<IInsertable<T>> action = null);

        /// <summary>
        /// 更新实体
        /// </summary>
        int Update(Action<IUpdateable<T>> action);

        /// <summary>
        /// 更新实体
        /// </summary>
        Task<int> UpdateAsync(Action<IUpdateable<T>> action);

        /// <summary>
        /// 更新实体
        /// </summary>
        bool Update(T entity, Action<IUpdateable<T>> action = null);

        /// <summary>
        /// 更新实体
        /// </summary>
        Task<bool> UpdateAsync(T entity, Action<IUpdateable<T>> action = null);

        /// <summary>
        /// 批量更新实体
        /// </summary>
        int Update(IEnumerable<T> entities, Action<IUpdateable<T>> action = null);

        /// <summary>
        /// 批量更新实体
        /// </summary>
        Task<int> UpdateAsync(IEnumerable<T> entities, Action<IUpdateable<T>> action = null);

        /// <summary>
        /// 保存实体
        /// </summary>
        bool Save(T entity, Action<ISaveable<T>> action = null);

        /// <summary>
        /// 批量保存实体
        /// </summary>
        int Save(IEnumerable<T> entities, Action<ISaveable<T>> action = null);

        /// <summary>
        /// 保存实体
        /// </summary>
        Task<bool> SaveAsync(T entity, Action<ISaveable<T>> action = null);

        /// <summary>
        /// 批量保存实体
        /// </summary>
        Task<int> SaveAsync(IEnumerable<T> entities, Action<ISaveable<T>> action = null);

        /// <summary>
        /// 移除实体
        /// </summary>
        int Delete(Action<IDeleteable<T>> action);

        /// <summary>
        /// 移除实体
        /// </summary>
        Task<int> DeleteAsync(Action<IDeleteable<T>> action);

        /// <summary>
        /// 移除实体
        /// </summary>
        bool Delete(params object[] keys);

        /// <summary>
        /// 移除实体
        /// </summary>
        Task<bool> DeleteAsync(params object[] keys);

        /// <summary>
        /// 移除实体
        /// </summary>
        bool Delete(T entity, Action<IDeleteable<T>> action = null);

        /// <summary>
        /// 移除实体
        /// </summary>
        Task<bool> DeleteAsync(T entity, Action<IDeleteable<T>> action = null);

        /// <summary>
        /// 移除实体
        /// </summary>
        int Delete(IEnumerable<T> entities, Action<IDeleteable<T>> action = null);

        /// <summary>
        /// 移除实体
        /// </summary>
        Task<int> DeleteAsync(IEnumerable<T> entities, Action<IDeleteable<T>> action = null);

        /// <summary>
        /// 查询实体
        /// </summary>
        IEnumerable<T> Select(Action<ISugarQueryable<T>> action = null);

        /// <summary>
        /// 查询实体
        /// </summary>
        Task<IEnumerable<T>> SelectAsync(Action<ISugarQueryable<T>> action = null);

        /// <summary>
        /// 查询实体
        /// </summary>
        T SelectOne(params object[] keys);

        /// <summary>
        /// 查询实体
        /// </summary>
        Task<T> SelectOneAsync(params object[] keys);

        /// <summary>
        /// 查询实体
        /// </summary>
        T SelectOne(Action<ISugarQueryable<T>> action);

        /// <summary>
        /// 查询实体
        /// </summary>
        Task<T> SelectOneAsync(Action<ISugarQueryable<T>> action);

        /// <summary>
        /// 计数
        /// </summary>
        long Count(Action<ISugarQueryable<T>> action = null);


        /// <summary>
        /// 计数
        /// </summary>
        Task<long> CountAsync(Action<ISugarQueryable<T>> action = null);
    }
}
