using QinSoft.Core.Common.Utils;
using QinSoft.Core.Configure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using QinSoft.Core.Data.MongoDB.Core;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace QinSoft.Core.Data.MongoDB
{
    /// <summary>
    /// mongodb管理器
    /// 集成缓存配置，支持多数据源
    /// </summary>
    public class MongoDBManager : IMongoDBManager
    {
        /// <summary>
        /// 日志
        /// </summary>
        protected ILogger logger;

        /// <summary>
        /// Database配置
        /// </summary>
        public MongoDBManagerConfig MongoDBManagerConfig { get; private set; }

        public MongoDBManager(MongoDBManagerConfig config) : this(config, NullLoggerFactory.Instance.CreateLogger<MongoDBManager>())
        {

        }

        public MongoDBManager(MongoDBManagerConfig config, ILogger logger)
        {
            ObjectUtils.CheckNull(config, "config");
            ObjectUtils.CheckNull(logger, "logger");
            MongoDBManagerConfig = config;
            this.logger = logger;
        }

        public MongoDBManager(MongoDBManagerOptions options, IConfiger configer) : this(options, configer, NullLoggerFactory.Instance)
        {
        }

        public MongoDBManager(MongoDBManagerOptions options, IConfiger configer, ILoggerFactory loggerFactory)
        {
            ObjectUtils.CheckNull(options, "options");
            ObjectUtils.CheckNull(configer, "configer");
            ObjectUtils.CheckNull(loggerFactory, "loggerFactory");
            MongoDBManagerConfig = configer.Get<MongoDBManagerConfig>(options.ConfigName, options.ConfigFormat);
            logger = loggerFactory.CreateLogger<MongoDBManager>();
        }

        public MongoDBManager(IOptions<MongoDBManagerOptions> optionsAccessor, IConfiger configer) : this(optionsAccessor, configer, NullLoggerFactory.Instance)
        {
        }

        public MongoDBManager(IOptions<MongoDBManagerOptions> optionsAccessor, IConfiger configer, ILoggerFactory loggerFactory)
        {
            ObjectUtils.CheckNull(optionsAccessor, "optionsAccessor");
            ObjectUtils.CheckNull(configer, "configer");
            ObjectUtils.CheckNull(loggerFactory, "loggerFactory");
            MongoDBManagerConfig = configer.Get<MongoDBManagerConfig>(optionsAccessor.Value.ConfigName, optionsAccessor.Value.ConfigFormat);
            logger = loggerFactory.CreateLogger<MongoDBManager>();
        }

        /// <summary>
        /// 获取缓存配置
        /// </summary>
        protected virtual MongoDBItemConfig GetMongoDBItemConfig(string name)
        {
            ObjectUtils.CheckNull(name, name);
            return MongoDBManagerConfig.GetByName(name);
        }

        /// <summary>
        /// 获取默认缓存配置
        /// </summary>
        protected virtual MongoDBItemConfig GetDefaultMongoDBItemConfig()
        {
            return GetMongoDBItemConfig(this.MongoDBManagerConfig.Primary);
        }

        /// <summary>
        /// 构建mongodb客户端实例
        /// </summary>
        protected virtual IMongoDBClient BuildClientFromConfig(MongoDBItemConfig config)
        {
            ObjectUtils.CheckNull(config, "config");
            return config.DefaultDBName.IsEmpty() ? new MongoDBClient(config.ConnectionString) : new MongoDBClient(config.ConnectionString, config.DefaultDBName);
        }

        /// <summary>
        /// 获取mongodb客户端
        /// </summary>
        public virtual IMongoDBClient GetMongoDB()
        {
            MongoDBItemConfig config = GetDefaultMongoDBItemConfig();
            if (config == null)
            {
                throw new MongoDBException("not found default mongodb client config");
            }

            IMongoDBClient client = BuildClientFromConfig(config);

            logger.LogDebug("get default mongodb client from config");

            return client;
        }

        /// <summary>
        /// 获取mongodb客户端
        /// </summary>
        public virtual async Task<IMongoDBClient> GetMongoDBAsync()
        {
            return await ExecuteUtils.ExecuteInTask(GetMongoDB);
        }

        /// <summary>
        /// 获取mongodb客户端
        /// </summary>
        public virtual IMongoDBClient GetMongoDB(string name)
        {
            ObjectUtils.CheckNull(name, "name");
            MongoDBItemConfig config = GetMongoDBItemConfig(name);
            if (config == null)
            {
                throw new MongoDBException(string.Format("not found mongodb client config:{0}", name));
            }

            IMongoDBClient client = BuildClientFromConfig(config);

            logger.LogDebug(string.Format("get mongodb client from config:{0}", name));

            return client;
        }

        /// <summary>
        /// 获取mongodb客户端
        /// </summary>
        public virtual async Task<IMongoDBClient> GetMongoDBAsync(string name)
        {
            return await ExecuteUtils.ExecuteInTask(GetMongoDB, name);
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
