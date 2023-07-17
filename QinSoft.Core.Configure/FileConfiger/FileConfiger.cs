using QinSoft.Core.Common.Utils;
using log4net;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QinSoft.Core.Configure.FileConfiger
{
    /// <summary>
    /// 文件配置器实现
    /// </summary>
    public class FileConfiger : Configer
    {
        /// <summary>
        /// 配置目录名称
        /// </summary>
        public string ConfigDirectoryName { get; private set; }

        /// <summary>
        /// 配置目录
        /// </summary>
        public DirectoryInfo ConfigDirectory { get; private set; }

        /// <summary>
        /// 配置管理文件名称
        /// </summary>
        public string ConfigManagerFileName { get; private set; }

        /// <summary>
        /// 配置管理文件名称
        /// </summary>
        public ConfigFormat ConfigManagerFileFormat { get; private set; }

        /// <summary>
        /// 文件读取器
        /// </summary>
        public FileConfigReader FileReader { get; private set; }

        /// <summary>
        /// 日志
        /// </summary>
        protected ILogger logger;

        /// <summary>
        /// 锁
        /// </summary>
        protected object SingleLockObj = new object();

        public FileConfiger(FileConfigerOptions options) : this(options, NullLoggerFactory.Instance)
        {
        }

        public FileConfiger(FileConfigerOptions options, ILoggerFactory loggerFactory) : base(options.ExpireIn)
        {
            if (options == null)
            {
                throw new ArgumentNullException("optionsAccessor");
            }
            if (loggerFactory == null)
            {
                throw new ArgumentNullException("loggerFactory");
            }
            ConfigDirectoryName = options.ConfigDirectoryName;
            ConfigDirectory = new DirectoryInfo(Path.GetFullPath(ConfigDirectoryName));
            ConfigManagerFileName = options.ConfigManagerFileName;
            ConfigManagerFileFormat = options.ConfigManagerFileFormat;
            logger = loggerFactory.CreateLogger<FileConfiger>();

            FileReader = new FileConfigReader();
        }

        public FileConfiger(IOptions<FileConfigerOptions> optionsAccessor) : this(optionsAccessor, NullLoggerFactory.Instance)
        {
        }

        public FileConfiger(IOptions<FileConfigerOptions> optionsAccessor, ILoggerFactory loggerFactory) : base(optionsAccessor.Value.ExpireIn)
        {
            if (optionsAccessor == null)
            {
                throw new ArgumentNullException("optionsAccessor");
            }
            if (loggerFactory == null)
            {
                throw new ArgumentNullException("loggerFactory");
            }
            ConfigDirectoryName = optionsAccessor.Value.ConfigDirectoryName;
            ConfigDirectory = new DirectoryInfo(Path.GetFullPath(ConfigDirectoryName));
            ConfigManagerFileName = optionsAccessor.Value.ConfigManagerFileName;
            ConfigManagerFileFormat = optionsAccessor.Value.ConfigManagerFileFormat;
            logger = loggerFactory.CreateLogger<FileConfiger>();

            FileReader = new FileConfigReader();
        }

        /// <summary>
        /// 获取配置管理文件
        /// </summary>
        protected virtual FileInfo GetConfigManagerFile()
        {
            //从配置目录获取
            return ConfigDirectory.GetFiles(ConfigManagerFileName + string.Format("(.({0}))?", string.Join("|", FileReader.GetSupportFileExtensions())), false).FirstOrDefault();
        }

        /// <summary>
        /// 获取配置管理
        /// </summary>
        protected virtual FileConfigManagerConfig GetConfigManagerConfig()
        {
            string key = "_ConfigManager";
            FileConfigManagerConfig config = GetConfigFromCache(key) as FileConfigManagerConfig;
            if (config == null)
            {
                FileInfo file = GetConfigManagerFile();
                config = FileReader.GetFileContent<FileConfigManagerConfig>(file, ConfigManagerFileFormat);
            }
            if (config != null)
            {
                StoreConfigToCache(key, config);
            }
            logger.LogDebug(string.Format("get config manager file:{0}", config.ToJson()));
            return config;
        }

        /// <summary>
        /// 获取配置文件
        /// </summary>
        public virtual FileInfo GetFile(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException("key");
            }
            //从配置管理文件获取
            FileConfigManagerConfig fileConfigManagerOptions = GetConfigManagerConfig();
            if (fileConfigManagerOptions != null)
            {
                FileConfigItemConfig configItem = fileConfigManagerOptions.GetByName(key);
                if (configItem != null)
                {
                    return new FileInfo(Path.GetFullPath(Path.Combine(ConfigDirectoryName, configItem.Path)));
                }
            }
            //从配置目录获取
            return ConfigDirectory.GetFiles(key + string.Format("(.({0}))?", string.Join("|", FileReader.GetSupportFileExtensions())), true).FirstOrDefault();
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        public override T Get<T>(string key, ConfigFormat format = ConfigFormat.XML)
        {
            lock (SingleLockObj)
            {
                if (string.IsNullOrEmpty(key))
                {
                    throw new ArgumentNullException("key");
                }
                T config = GetConfigFromCache(key) as T;
                if (config == null)
                {
                    FileInfo file = GetFile(key);
                    config = FileReader.GetFileContent<T>(file, format);
                }
                if (config != null)
                {
                    StoreConfigToCache(key, config);
                }
                logger.LogDebug(string.Format("get config file {0}:{1}", key, config.ToJson()));

                if (config == null)
                {
                    throw new ConfigureException(string.Format("not get config file:{0}", key));
                }

                return config;
            }
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        public override Task<T> GetAsync<T>(string key, ConfigFormat format = ConfigFormat.XML)
        {
            return Task.Factory.StartNew(() => Get<T>(key, format));
        }
    }
}
