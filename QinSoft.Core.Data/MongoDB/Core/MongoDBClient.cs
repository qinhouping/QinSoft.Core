using QinSoft.Core.Common.Utils;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Data.MongoDB.Core
{
    /// <summary>
    /// Mongo数据库客户端
    /// </summary>
    public class MongoDBClient : MongoClient, IMongoDBClient
    {
        /// <summary>
        /// 默认数据库名称
        /// </summary>
        public string DefaultDBName { get; protected set; }

        public MongoDBClient() : base()
        {
        }

        public MongoDBClient(MongoClientSettings settings) : base(settings)
        {
        }

        public MongoDBClient(MongoClientSettings settings, string defaultDBName) : base(settings)
        {
            ArgumentUtils.CheckNull(defaultDBName, "defaultDBName");
            DefaultDBName = defaultDBName;
        }

        public MongoDBClient(MongoUrl url) : base(url)
        {
            this.DefaultDBName = url.DatabaseName;
        }

        public MongoDBClient(string connectionString) : base(connectionString)
        {
            this.DefaultDBName = MongoUrl.Create(connectionString).DatabaseName;
        }

        public MongoDBClient(string connectionString, string defaultDBName) : base(connectionString)
        {
            ArgumentUtils.CheckNull(defaultDBName, "defaultDBName");
            this.DefaultDBName = defaultDBName;
        }


        /// <summary>
        /// 获取默认数据库
        /// </summary>
        public virtual IMongoDatabase GetDatabase(MongoDatabaseSettings settings = null)
        {
            ArgumentUtils.CheckNull(DefaultDBName, () => new MongoDBException("get default mongo database error"));
            return this.GetDatabase(DefaultDBName, settings);
        }

        public virtual void Dispose()
        {

        }
    }
}
