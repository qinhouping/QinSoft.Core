﻿using QinSoft.Core.Configure;
using QinSoft.Core.Configure.FileConfiger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MySql.Data.MySqlClient;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using QinSoft.Core.Common.Utils;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using QinSoft.Core.Data.Database;
using System.Data.SqlClient;
using MongoDB.Driver;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using QinSoft.Core.Data.MongoDB;
using QinSoft.Core.Data.MongoDB.Core;
using MongoDB.Bson.Serialization.Attributes;

namespace QinSoft.Core.Test
{
    [TestClass]
    public class DataUnitTest
    {
        public ThreadLocal<string> ThreadVar { get; set; }

        [TestMethod]
        public void TestDatabaseManager()
        {
            IConfiger configer = new FileConfiger(new FileConfigerOptions());
            DatabaseManagerConfig databaseManagerConfig = configer.Get<DatabaseManagerConfig>("DatabaseManager");
            using (IDatabaseManager databaseManager = new DatabaseManager(databaseManagerConfig))
            {
                using (ISqlSugarClient client = databaseManager.GetDatabase())
                {
                    Assert.IsTrue(client.Queryable<TestTable>().ToList().Count > 0);
                }
            }
        }

        [TestMethod]
        public void TestDatabaseRepository()
        {
            ITestTableRepository repository = Programe.ServiceProvider.GetService<ITestTableRepository>();
            var model = new TestTable()
            {
                id = "daf7f265-c48b-444c-a4d9-2c584a28ace2",
                va = DateTime.Now.ToString()
            };

            if (repository.Count(query => query.Where(t => t.id.Equals("daf7f265-c48b-444c-a4d9-2c584a28ace2"))) == 0)
            {
                Assert.AreEqual(repository.Insert(model), true);
            }
            else
            {
                Assert.AreEqual(repository.Update(model), true);
            }

            TestTable result = repository.SelectOne((query) => { query.Where(t => t.id.Equals("daf7f265-c48b-444c-a4d9-2c584a28ace2")); });

            Assert.AreEqual(result.va, model.va);

            Assert.AreNotEqual(repository.Count(), 0);

            Assert.AreEqual(repository.Delete((delete) => { delete.Where(t => t.id.Equals("daf7f265-c48b-444c-a4d9-2c584a28ace2")); }), 1);
        }

        [TestMethod]
        public void TestMongoDBManager()
        {
            IConfiger configer = new FileConfiger(new FileConfigerOptions());
            MongoDBManagerConfig mongodbManagerConfig = configer.Get<MongoDBManagerConfig>("MongoDBManagerConfig");
            using (IMongoDBManager mongodbManager = new MongoDBManager(mongodbManagerConfig))
            {
                using (IMongoDBClient client = mongodbManager.GetMongoDB())
                {
                    Assert.AreEqual(client.GetDatabase().GetCollection<TestTable>("t2").Find(u => true).ToList().Count, 0);
                }
            }
        }

        [TestMethod]
        public void TestMongoDBRepository()
        {
            ITestTableMongoDBRepository repository = Programe.ServiceProvider.GetService<ITestTableMongoDBRepository>();
            var model = new TestTable()
            {
                id = "daf7f265-c48b-444c-a4d9-2c584a28ace2",
                va = DateTime.Now.ToString()
            };

            if (repository.FindOne(model) == null)
            {
                Assert.AreEqual(repository.InsertOne(model), true);
            }
            else
            {
                Assert.AreEqual(repository.UpdateOne(model), true);
            }

            TestTable result = repository.FindOne(model);

            Assert.AreEqual(result.va, model.va);

            Assert.AreEqual(repository.DeleteOne(model), true);
        }
    }

    [MongoDBCollection("t")]
    [SugarTable("t")]
    public class TestTable
    {
        [BsonId]
        [SugarColumn(IsPrimaryKey = true)]
        public string id { get; set; }

        [BsonElement("value")]
        [SugarColumn(ColumnName = "value")]
        public string va { get; set; }

        [BsonIgnore]
        [SugarColumn(IsIgnore = true)]
        public string ext { get; set; }
    }

    public interface ITestTableRepository : IDatabaseRepository<TestTable>
    {

    }

    [DatabaseContext(true)]
    public class TestTableRepository : DatabaseRepository<TestTable>, ITestTableRepository
    {

    }

    public interface ITestTableMongoDBRepository : IMongoDBRepository<TestTable>
    {

    }

    public class TestTableMongoDBRepository : MongoDBRepository<TestTable>, ITestTableMongoDBRepository
    {

    }
}
