using QinSoft.Core.Data.MongoDB.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Data.MongoDB
{
    /// <summary>
    /// mongodb上下文
    /// </summary>
    public interface IMongoDBContext : IDisposable
    {
        /// <summary>
        /// 获取mongodb客户端
        /// </summary>
        IMongoDBClient Get();

        /// <summary>
        /// 获取mongodb客户端
        /// </summary>
        IMongoDBClient Get(string name);
    }
}
