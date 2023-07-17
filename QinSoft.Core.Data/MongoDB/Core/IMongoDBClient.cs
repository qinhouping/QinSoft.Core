using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Data.MongoDB.Core
{
    public interface IMongoDBClient : IMongoClient, IDisposable
    {
        string DefaultDBName { get; }

        IMongoDatabase GetDatabase(MongoDatabaseSettings settings = null);
    }
}
