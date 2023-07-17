using QinSoft.Core.Data.MongoDB.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace QinSoft.Core.Data.MongoDB
{
    public interface IMongoDBManager
    {
        /// <summary>
        /// 获取mongodb客户端
        /// </summary>
        IMongoDBClient GetMongoDB();

        /// <summary>
        /// 获取mongodb客户端
        /// </summary>
        Task<IMongoDBClient> GetMongoDBAsync();

        /// <summary>
        /// 获取mongodb客户端
        /// </summary>
        IMongoDBClient GetMongoDB(string name);

        /// <summary>
        /// 获取mongodb客户端
        /// </summary>
        Task<IMongoDBClient> GetMongoDBAsync(string name);
    }
}
