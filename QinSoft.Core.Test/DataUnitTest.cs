using QinSoft.Core.Configure;
using QinSoft.Core.Configure.FileConfiger;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
using QinSoft.Core.Data.Elasticsearch;
using Nest;
using MongoDB.Bson;
using System.Linq.Expressions;
using QinSoft.Core.Data.Zookeeper;
using org.apache.zookeeper;
using QinSoft.Core.Data.Zookeeper.Core;
using org.apache.zookeeper.data;
using QinSoft.Core.Data.Solr;
using SolrNet;
using SolrNet.Attributes;
using SolrNet.Commands.Parameters;
using System.Linq;
using QinSoft.Core.Data.InfluxDB;
using QinSoft.Core.Data.InfuxDB.Core;
using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using InfluxDB.Client.Core.Flux.Domain;
using InfluxDB.Client.Core;
using InfluxDB.Client.Linq;

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
                    Assert.AreEqual(client.Queryable<Project>().Count(), 0);
                }
            }
        }

        [TestMethod]
        public void TestDatabaseRepository()
        {
            IProjectRepository repository = Programe.ServiceProvider.GetService<IProjectRepository>();
            var model = new Project()
            {
                Id = "qinsoft.core",
                ProjectName = "QinSoft 核心库",
                ProjectDescription = "核心库"
            };

            Assert.IsTrue(repository.Insert(model, i => i.IgnoreColumns(true)));

            Assert.AreEqual(repository.Count(query => query.Where(t => t.Id.Equals("qinsoft.core"))), 1);

            Project result = repository.SelectOne(query => { query.Where(t => t.Id.Equals("qinsoft.core")); });

            Assert.AreEqual(result.ProjectName, model.ProjectName);
            Assert.AreEqual(result.ProjectDescription, model.ProjectDescription);

            Assert.AreEqual(repository.Update(u => u.SetColumns(t => new Project() { ProjectDescription = "核心库2" }).Where(t => t.Id.Equals("qinsoft.core"))), 1);

            Assert.AreEqual(repository.Delete(delete => { delete.Where(t => t.Id.Equals("qinsoft.core")); }), 1);
        }

        [TestMethod]
        public void TestDatabaseManagerOfClickHouse()
        {
            IConfiger configer = new FileConfiger(new FileConfigerOptions());
            DatabaseManagerConfig databaseManagerConfig = configer.Get<DatabaseManagerConfig>("DatabaseManager");
            using (IDatabaseManager databaseManager = new DatabaseManager(databaseManagerConfig))
            {
                using (ISqlSugarClient client = databaseManager.GetDatabase("ch_test"))
                {
                    Assert.AreEqual(client.Queryable<Project>().Count(), 0);
                }
            }
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
                    Assert.AreEqual(client.GetDatabase().GetCollection<Project>("project").Find(u => true).ToList().Count, 0);
                }
            }
        }

        [TestMethod]
        public void TestMongoDBRepository()
        {
            IProjectMongoDBRepository repository = Programe.ServiceProvider.GetService<IProjectMongoDBRepository>();
            var model = new Project()
            {
                Id = "qinsoft.core",
                ProjectName = "QinSoft 核心库",
                ProjectDescription = "核心库",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };

            Assert.IsTrue(repository.InsertOne(model));

            Assert.AreEqual(repository.Count((Expression<Func<Project, bool>>)(t => t.Id.Equals("qinsoft.core"))), 1);

            Project result = repository.FindOne(model);

            Assert.AreEqual(result.ProjectName, model.ProjectName);
            Assert.AreEqual(result.ProjectDescription, model.ProjectDescription);

            UpdateDefinition<Project> update = Builders<Project>.Update.Combine(Builders<Project>.Update.Set(t => t.ProjectDescription, "核心库2")
                , Builders<Project>.Update.Set(t => t.UpdateTime, DateTime.Now));
            Assert.IsTrue(repository.UpdateOne((Expression<Func<Project, bool>>)(t => t.Id.Equals("qinsoft.core")), update));

            Assert.AreEqual(repository.DeleteOne(model), true);
        }

        [TestMethod]
        public void TestElasticsearchManager()
        {
            IConfiger configer = new FileConfiger(new FileConfigerOptions());
            ElasticsearchManagerConfig elasticsearchManagerConfig = configer.Get<ElasticsearchManagerConfig>("ElasticsearchManagerConfig");
            using (IElasticsearchManager elasticsearchManager = new ElasticsearchManager(elasticsearchManagerConfig))
            {
                IElasticClient client = elasticsearchManager.GetElasticsearch();
                Assert.AreEqual(client.Count<Project>(d => d.Index("project")).Count, 0);
            }
        }

        [TestMethod]
        public void TestElasticsearchRespository()
        {
            IProjectElasticsearchRepository repository = Programe.ServiceProvider.GetService<IProjectElasticsearchRepository>();
            var model = new Project()
            {
                Id = "qinsoft.core",
                ProjectName = "QinSoft 核心库",
                ProjectDescription = "核心库",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };

            Assert.IsTrue(repository.Index(model).Item1);

            Assert.AreEqual(repository.Count(q => q.Match(m => m.Field(t => t.Id).Query("qinsoft.core"))), 1);

            Project result = repository.Get("qinsoft.core");
            Assert.AreEqual(result.ProjectName, model.ProjectName);
            Assert.AreEqual(result.ProjectDescription, model.ProjectDescription);

            Assert.IsTrue(repository.Update<PartProject>("qinsoft.core", new PartProject() { ProjectDescription = "核心库2", UpdateTime = DateTime.Now }));

            Assert.AreEqual(repository.Delete("qinsoft.core"), true);
        }

        [TestMethod]
        public void TestZookeeperManager()
        {
            IConfiger configer = new FileConfiger(new FileConfigerOptions());
            ZookeeperManagerConfig zookeeperManagerConfig = configer.Get<ZookeeperManagerConfig>("ZookeeperManagerConfig");
            using (IZookeeperManager zookeeperManager = new ZookeeperManager(zookeeperManagerConfig))
            {
                IZookeeper client = zookeeperManager.GetZookeeper();
                client.Dispose();
                Assert.IsNull(client.Exists("/not_exists", false));
                client.Create("/qinsoft", null, CreateMode.EPHEMERAL);

                client.Exists("/qinsoft", new QinSoftWatcher());

                string value = Guid.NewGuid().ToString();
                Assert.IsNotNull(client.SetData("/qinsoft", value));

                string res = client.GetData("/qinsoft").Item2;
                Assert.AreEqual(res, value);

                client.Delete("/qinsoft");
            }
        }

        [TestMethod]
        public void TestSolrManager()
        {
            IConfiger configer = new FileConfiger(new FileConfigerOptions());
            SolrManagerConfig solrManagerConfig = configer.Get<SolrManagerConfig>("SolrManagerConfig");
            using (ISolrManager solrManager = new SolrManager(solrManagerConfig))
            {
                ISolrOperations<Project> client = solrManager.GetSolr<Project>("project");
                Assert.AreEqual(client.Ping().Status, 0);
            }
        }


        [TestMethod]
        public void TestSolrRespository()
        {
            IProjectSolrRepository repository = Programe.ServiceProvider.GetService<IProjectSolrRepository>();
            var model = new Project()
            {
                Id = "qinsoft.core",
                ProjectName = "QinSoft 核心库",
                ProjectDescription = "核心库",
                CreateTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };

            Assert.IsTrue(repository.Add(model));
            repository.Commit();

            ISolrQuery query = new SolrQuery("id:qinsoft.core");
            var result = repository.Query(query, (QueryOptions)null).First();
            Assert.IsNotNull(result);
            Assert.AreEqual(result.ProjectName, model.ProjectName);
            Assert.AreEqual(result.ProjectDescription, model.ProjectDescription);

            Assert.AreEqual(repository.Delete("qinsoft.core"), true);
            repository.Commit();
        }

        [TestMethod]
        public void TestInfluxDBManager()
        {
            IConfiger configer = new FileConfiger(new FileConfigerOptions());
            InfluxDBManagerConfig influxdbManagerConfig = configer.Get<InfluxDBManagerConfig>("InfluxDBManagerConfig");
            using (IInfluxDBManager solrManager = new InfluxDBManager(influxdbManagerConfig))
            {
                IInfluxClient client = solrManager.GetInflux();
                IWriteApi write = client.GetWriteApi();
                PointData point = PointData
                    .Measurement("project")
                    .Tag("project_id", "qinsoft.core")
                    .Field("rate", 30)
                    .Timestamp(DateTime.Now, WritePrecision.Ns);
                write.WritePoint(point);
                //write.WriteRecord(@"project,project_id=qinsoft.core rate=30 " + DateTime.Now.ToTimestamp(DateTimeUtils.TimestampPrecision.Ns), WritePrecision.Ns);

                IQueryApi query = client.GetQueryApi();
                string q = @"from(bucket:""qinsoft"") 
                                |> range(start: -1h) 
                                |> filter(fn:(r)=>r._measurement==""project"")";
                List<FluxTable> tables = query.QueryAsync(q).Result;

                Assert.AreEqual(1, tables.Count);

                IDeleteApi delete = client.GetDeleteApi();
                delete.Delete(DateTime.Today, DateTime.Today.AddDays(1), @"_measurement=""project""", "qinsoft", "qinsoft").Wait();
            }
        }
    }

    public class QinSoftWatcher : Watcher
    {
        public override async Task process(WatchedEvent @event)
        {
            await Task.Delay(10);
            Console.WriteLine(@event.ToString());
        }
    }

    [SugarTable("project")]
    [MongoDBCollection("project")]
    [ElasticsearchIndex("project")]
    [SolrCore("project")]
    [Measurement("project")]
    public class Project
    {
        [BsonId]
        [SugarColumn(IsPrimaryKey = true)]
        [Keyword(Name = "id")]
        [SolrUniqueKey("id")]
        [Column("id", IsTag = true)]
        public string Id { get; set; }

        [BsonElement("project_name")]
        [SugarColumn(ColumnName = "project_name")]
        [Text(Name = "project_name")]
        [SolrField("project_name")]
        [Column("project_name", IsTag = true)]
        public string ProjectName { get; set; }

        [BsonElement("project_description")]
        [SugarColumn(ColumnName = "project_description")]
        [Text(Name = "project_description")]
        [SolrField("project_description")]
        public string ProjectDescription { get; set; }

        [BsonElement("create_time")]
        [SugarColumn(ColumnName = "create_time")]
        [Date(Name = "create_time")]
        [SolrField("create_time")]
        [Column("create_time", IsTimestamp = true)]
        public DateTime? CreateTime { get; set; }

        [BsonElement("update_time")]
        [SugarColumn(ColumnName = "update_time")]
        [Date(Name = "update_time")]
        [SolrField("update_time")]
        public DateTime? UpdateTime { get; set; }
    }

    public class PartProject
    {
        [Text(Name = "project_description")]
        public string ProjectDescription { get; set; }

        [Date(Name = "update_time")]
        public DateTime? UpdateTime { get; set; }
    }

    public interface IProjectRepository : IDatabaseRepository<Project>
    {

    }

    [DatabaseContext(true)]
    public class ProjectRepository : DatabaseRepository<Project>, IProjectRepository
    {

    }

    public interface IProjectMongoDBRepository : IMongoDBRepository<Project>
    {

    }

    public class ProjectMongoDBRepository : MongoDBRepository<Project>, IProjectMongoDBRepository
    {

    }

    public interface IProjectElasticsearchRepository : IElasticsearchRepository<Project>
    {

    }

    public class ProjectElasticsearchRepository : ElasticsearchRepository<Project>, IProjectElasticsearchRepository
    {

    }

    public interface IProjectSolrRepository : ISolrRepository<Project>
    {

    }

    public class ProjectSolrRepository : SolrRepository<Project>, IProjectSolrRepository
    {

    }
}
