using QinSoft.Core.Common.Utils;
using QinSoft.Core.Configure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using org.apache.zookeeper;
using QinSoft.Core.Data.Zookeeper.Core;

namespace QinSoft.Core.Data.Zookeeper
{
    /// <summary>
    /// zookeeper管理器
    /// 集成缓存配置，支持多数据源
    /// </summary>
    public class ZookeeperManager : IZookeeperManager
    {
        /// <summary>
        /// 日志
        /// </summary>
        protected ILogger logger;

        /// <summary>
        /// zookeeper配置
        /// </summary>
        public ZookeeperManagerConfig ZookeeperManagerConfig { get; private set; }

        /// <summary>
        /// 缓存字典
        /// </summary>
        protected ConcurrentDictionary<string, IZookeeper> CacheDictionary;

        public ZookeeperManager(ZookeeperManagerConfig config) : this(config, NullLoggerFactory.Instance.CreateLogger<ZookeeperManager>())
        {

        }

        public ZookeeperManager(ZookeeperManagerConfig config, ILogger logger)
        {
            ObjectUtils.CheckNull(config, "config");
            ObjectUtils.CheckNull(logger, "logger");
            this.CacheDictionary = new ConcurrentDictionary<string, IZookeeper>();
            ZookeeperManagerConfig = config;
            this.logger = logger;
        }

        public ZookeeperManager(ZookeeperManagerOptions options, IConfiger configer) : this(options, configer, NullLoggerFactory.Instance)
        {
        }

        public ZookeeperManager(ZookeeperManagerOptions options, IConfiger configer, ILoggerFactory loggerFactory)
        {
            ObjectUtils.CheckNull(options, "options");
            ObjectUtils.CheckNull(configer, "configer");
            ObjectUtils.CheckNull(loggerFactory, "loggerFactory");
            this.CacheDictionary = new ConcurrentDictionary<string, IZookeeper>();
            ZookeeperManagerConfig = configer.Get<ZookeeperManagerConfig>(options.ConfigName, options.ConfigFormat);
            logger = loggerFactory.CreateLogger<ZookeeperManager>();
        }

        public ZookeeperManager(IOptions<ZookeeperManagerOptions> optionsAccessor, IConfiger configer) : this(optionsAccessor, configer, NullLoggerFactory.Instance)
        {
        }

        public ZookeeperManager(IOptions<ZookeeperManagerOptions> optionsAccessor, IConfiger configer, ILoggerFactory loggerFactory)
        {
            ObjectUtils.CheckNull(optionsAccessor, "optionsAccessor");
            ObjectUtils.CheckNull(configer, "configer");
            ObjectUtils.CheckNull(loggerFactory, "loggerFactory");
            this.CacheDictionary = new ConcurrentDictionary<string, IZookeeper>();
            ZookeeperManagerConfig = configer.Get<ZookeeperManagerConfig>(optionsAccessor.Value.ConfigName, optionsAccessor.Value.ConfigFormat);
            logger = loggerFactory.CreateLogger<ZookeeperManager>();
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        protected virtual ZookeeperItemConfig GetZookeeperItemConfig(string name)
        {
            ObjectUtils.CheckNull(name, name);
            return ZookeeperManagerConfig.GetByName(name);
        }

        /// <summary>
        /// 获取默认配置
        /// </summary>
        protected virtual ZookeeperItemConfig GetDefaultZookeeperItemConfig()
        {
            return GetZookeeperItemConfig(this.ZookeeperManagerConfig.Primary);
        }

        /// <summary>
        /// 构建zookeeper客户端实例
        /// </summary>
        protected virtual IZookeeper BuildClientFromConfig(ZookeeperItemConfig config)
        {
            ObjectUtils.CheckNull(config, "config");
            Core.Zookeeper zooKeeper = new Core.Zookeeper(config.ConnectionString, config.SessionTimeout);
            if(!config.AuthInfos.IsEmpty()){
               foreach(ZookeeperAuthConfig authConfig in config.AuthInfos )
               {
                    zooKeeper.addAuthInfo(authConfig.Schema, Encoding.Default.GetBytes(authConfig.Auth));
               }
            }
            return zooKeeper;
        }

        /// <summary>
        /// 获取zookeeper客户端
        /// </summary>
        public virtual IZookeeper GetZookeeper()
        {
            ZookeeperItemConfig config = GetDefaultZookeeperItemConfig();
            if (config == null)
            {
                throw new ZookeeperException("not found default zookeeper client config");
            }

            IZookeeper client = CacheDictionary.GetOrAdd(config.Name, key => BuildClientFromConfig(config));

            logger.LogDebug("get default zookeeper client from config");

            return client;
        }

        /// <summary>
        /// 获取zookeeper客户端
        /// </summary>
        public virtual async Task<IZookeeper> GetZookeeperAsync()
        {
            return await ExecuteUtils.ExecuteInTask(GetZookeeper);
        }

        /// <summary>
        /// 获取zookeeper客户端
        /// </summary>
        public virtual IZookeeper GetZookeeper(string name)
        {
            ObjectUtils.CheckNull(name, "name");
            ZookeeperItemConfig config = GetZookeeperItemConfig(name);
            if (config == null)
            {
                throw new ZookeeperException(string.Format("not found zookeeper client config:{0}", name));
            }

            IZookeeper client =CacheDictionary.GetOrAdd(config.Name, key=> BuildClientFromConfig(config));

            logger.LogDebug(string.Format("get zookeeper client from config:{0}", name));

            return client;
        }

        /// <summary>
        /// 获取zookeeper客户端
        /// </summary>
        public virtual async Task<IZookeeper> GetZookeeperAsync(string name)
        {
            return await ExecuteUtils.ExecuteInTask(GetZookeeper, name);
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
