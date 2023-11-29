using QinSoft.Core.Common.Utils;
using QinSoft.Core.Configure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static NodaTime.TimeZones.ZoneEqualityComparer;

namespace QinSoft.Core.Data.Database
{
    /// <summary>
    /// Database管理器
    /// 集成缓存配置，支持多数据源
    /// </summary>
    public class DatabaseManager : IDatabaseManager
    {
        /// <summary>
        /// 日志
        /// </summary>
        protected ILogger logger;

        /// <summary>
        /// Database配置
        /// </summary>
        public DatabaseManagerConfig DatabaseManagerConfig { get; private set; }

        public DatabaseManager(DatabaseManagerConfig config) : this(config, NullLoggerFactory.Instance.CreateLogger<DatabaseManager>())
        {

        }

        public DatabaseManager(DatabaseManagerConfig config, ILogger logger)
        {
            ObjectUtils.CheckNull(config, nameof(config));
            ObjectUtils.CheckNull(logger, nameof(logger));
            DatabaseManagerConfig = config;
            this.logger = logger;
        }

        public DatabaseManager(DatabaseManagerOptions options, IConfiger configer) : this(options, configer, NullLoggerFactory.Instance)
        {
        }

        public DatabaseManager(DatabaseManagerOptions options, IConfiger configer, ILoggerFactory loggerFactory)
        {
            ObjectUtils.CheckNull(options, nameof(options));
            ObjectUtils.CheckNull(configer, nameof(configer));
            ObjectUtils.CheckNull(loggerFactory, nameof(loggerFactory));
            DatabaseManagerConfig = configer.Get<DatabaseManagerConfig>(options.ConfigName, options.ConfigFormat);
            logger = loggerFactory.CreateLogger<DatabaseManager>();
        }

        public DatabaseManager(IOptions<DatabaseManagerOptions> optionsAccessor, IConfiger configer) : this(optionsAccessor, configer, NullLoggerFactory.Instance)
        {
        }

        public DatabaseManager(IOptions<DatabaseManagerOptions> optionsAccessor, IConfiger configer, ILoggerFactory loggerFactory)
        {
            ObjectUtils.CheckNull(optionsAccessor, nameof(optionsAccessor));
            ObjectUtils.CheckNull(configer, nameof(configer));
            ObjectUtils.CheckNull(loggerFactory, nameof(loggerFactory));
            DatabaseManagerConfig = configer.Get<DatabaseManagerConfig>(optionsAccessor.Value.ConfigName, optionsAccessor.Value.ConfigFormat);
            logger = loggerFactory.CreateLogger<DatabaseManager>();
        }

        /// <summary>
        /// 获取缓存配置
        /// </summary>
        protected virtual DatabaseItemConfig GetDatabaseItemConfig(string name)
        {
            ObjectUtils.CheckNull(name, name);
            return DatabaseManagerConfig.GetByName(name);
        }

        /// <summary>
        /// 获取默认缓存配置
        /// </summary>
        protected virtual DatabaseItemConfig GetDefaultDatabaseItemConfig()
        {
            return GetDatabaseItemConfig(this.DatabaseManagerConfig.Primary);
        }

        /// <summary>
        /// 构建Database客户端实例
        /// </summary>
        protected virtual ISqlSugarClient BuildClientFromConfig(DatabaseItemConfig config)
        {
            return new SqlSugarClient(config.Configs?.Select(u =>
            {
                ConnectionConfig connectionConfig = new ConnectionConfig();
                connectionConfig.ConfigId = u.ConfigId;
                connectionConfig.DbType = u.DbType.ParseEnum<DbType>().Value;
                connectionConfig.ConnectionString = u.ConnectionString;
                connectionConfig.IsAutoCloseConnection = u.IsAutoCloseConnection;
                connectionConfig.SlaveConnectionConfigs = u.Slaves?.Select(s =>
                {
                    SlaveConnectionConfig slaveConnectionConfig = new SlaveConnectionConfig();
                    slaveConnectionConfig.HitRate = s.HitRate;
                    slaveConnectionConfig.ConnectionString = s.ConnectionString;
                    return slaveConnectionConfig;
                }).ToList();
                if (u.MoreSetting != null)
                {
                    ConnMoreSettings moreSettings = new ConnMoreSettings();
                    moreSettings.IsAutoRemoveDataCache = u.MoreSetting.IsAutoRemoveDataCache;
                    moreSettings.IsWithNoLockQuery = u.MoreSetting.IsWithNoLockQuery;
                    moreSettings.IsWithNoLockSubquery = u.MoreSetting.IsWithNoLockSubquery;
                    moreSettings.DisableNvarchar = u.MoreSetting.DisableNvarchar;
                    moreSettings.DisableMillisecond = u.MoreSetting.DisableMillisecond;
                    moreSettings.PgSqlIsAutoToLower = u.MoreSetting.PgSqlIsAutoToLower;
                    moreSettings.PgSqlIsAutoToLowerCodeFirst = u.MoreSetting.PgSqlIsAutoToLowerCodeFirst;
                    moreSettings.IsAutoToUpper = u.MoreSetting.IsAutoToUpper;
                    moreSettings.DefaultCacheDurationInSeconds = u.MoreSetting.DefaultCacheDurationInSeconds;
                    moreSettings.TableEnumIsString = u.MoreSetting.TableEnumIsString;
                    moreSettings.DbMinDate = u.MoreSetting.DbMinDate;
                    moreSettings.IsNoReadXmlDescription = u.MoreSetting.IsNoReadXmlDescription;
                    moreSettings.SqlServerCodeFirstNvarchar = u.MoreSetting.SqlServerCodeFirstNvarchar;
                    moreSettings.IsAutoUpdateQueryFilter = u.MoreSetting.IsAutoUpdateQueryFilter;
                    moreSettings.EnableModelFuncMappingColumn = u.MoreSetting.EnableModelFuncMappingColumn;
                    moreSettings.EnableOracleIdentity = u.MoreSetting.EnableOracleIdentity;
                    connectionConfig.MoreSettings = moreSettings;
                }
                connectionConfig.AopEvents = new AopEvents()
                {
                    //SQL执行前事件。可在这里查看生成的sql
                    OnLogExecuting = (sql, pars) =>
                    {
                        StringBuilder msg = new StringBuilder();
                        msg.AppendLine("SqlSugarClient executing:");
                        msg.AppendLine("sql:" + sql);
                        msg.AppendLine("parametres:" + pars.ToJson());
                        logger?.LogDebug(msg.ToString());
                    },
                    OnError = (e) =>
                    {
                        logger?.LogError("SqlSugarClient execute error", new DatabaseException("database error", e));
                    }
                };
                return connectionConfig;
            }).ToList());
        }

        /// <summary>
        /// 获取数据库客户端
        /// </summary>
        public virtual ISqlSugarClient GetDatabase()
        {
            DatabaseItemConfig config = GetDefaultDatabaseItemConfig();
            if (config == null)
            {
                throw new DatabaseException("not found default database client config");
            }

            ISqlSugarClient client = BuildClientFromConfig(config);

            logger.LogDebug("get default database client from config");

            return client;
        }

        /// <summary>
        /// 获取数据库客户端
        /// </summary>
        public virtual async Task<ISqlSugarClient> GetDatabaseAsync()
        {
            return await ExecuteUtils.ExecuteInTask(GetDatabase);
        }

        /// <summary>
        /// 获取数据库客户端
        /// </summary>
        public virtual ISqlSugarClient GetDatabase(string name)
        {
            ObjectUtils.CheckNull(name, nameof(name));
            DatabaseItemConfig config = GetDatabaseItemConfig(name);
            if (config == null)
            {
                throw new DatabaseException(string.Format("not found database client config:{0}", name));
            }

            ISqlSugarClient client = BuildClientFromConfig(config);

            logger.LogDebug(string.Format("get database client from config:{0}", name));

            return client;
        }


        /// <summary>
        /// 获取数据库客户端
        /// </summary>
        public virtual async Task<ISqlSugarClient> GetDatabaseAsync(string name)
        {
            return await ExecuteUtils.ExecuteInTask(GetDatabase, name);
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
