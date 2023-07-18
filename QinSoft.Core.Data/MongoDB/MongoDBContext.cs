using QinSoft.Core.Common.Utils;
using QinSoft.Core.Data.MongoDB.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace QinSoft.Core.Data.MongoDB
{
    /// <summary>
    /// mongodb上下文
    /// </summary>
    public class MongoDBContext : IMongoDBContext
    {
        /// </summary>
        /// 单例模式，内部使用
        /// </summary>
        internal static IMongoDBContext Instance { get; set; }

        protected IMongoDBManager mongodbManager { get; set; }

        protected AsyncLocal<ConcurrentDictionary<string, IMongoDBClient>> Context { get; set; }

        /// <summary>
        /// 默认上下文
        /// </summary>
        protected AsyncLocal<IMongoDBClient> DefaultContext { get; set; }

        public MongoDBContext(IMongoDBManager mongodbManager)
        {
            ObjectUtils.CheckNull(mongodbManager, "mongodbManager");
            this.Context = new AsyncLocal<ConcurrentDictionary<string, IMongoDBClient>>();
            this.DefaultContext = new AsyncLocal<IMongoDBClient>();
            this.mongodbManager = mongodbManager;
            Instance = this;
        }

        /// <summary>
        /// 获取mongodb客户端
        /// </summary>
        public virtual IMongoDBClient Get()
        {
            if (DefaultContext.Value == null)
            {
                DefaultContext.Value = mongodbManager.GetMongoDB();
            }
            return DefaultContext.Value;
        }

        /// <summary>
        /// 获取mongodb客户端
        /// </summary>
        public virtual IMongoDBClient Get(string name)
        {
            if (Context.Value == null)
            {
                Context.Value = new ConcurrentDictionary<string, IMongoDBClient>();
            }
            return Context.Value.GetOrAdd(name, mongodbManager.GetMongoDB(name));
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
