using SqlSugar;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QinSoft.Core.Data.Database
{
    /// <summary>
    /// 数据库管理器接口
    /// </summary>
    public interface IDatabaseManager : IDisposable
    {
        /// <summary>
        /// 获取数据库客户端
        /// </summary>
        ISqlSugarClient GetDatabase();

        /// <summary>
        /// 获取数据库客户端
        /// </summary>
        Task<ISqlSugarClient> GetDatabaseAsync();

        /// <summary>
        /// 获取数据库客户端
        /// </summary>
        ISqlSugarClient GetDatabase(string name);

        /// <summary>
        /// 获取数据库客户端
        /// </summary>
        Task<ISqlSugarClient> GetDatabaseAsync(string name);
    }
}
