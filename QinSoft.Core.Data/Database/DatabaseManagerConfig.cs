using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace QinSoft.Core.Data.Database
{
    /// <summary>
    /// 数据库管理器配置
    /// </summary>
    [XmlRoot("databaseManager", Namespace = "http://www.qinsoft.com")]
    public class DatabaseManagerConfig
    {
        /// <summary>
        /// 默认数据库配置
        /// </summary>
        [XmlAttribute("primary")]
        [JsonProperty("primary")]
        public string Primary { get; set; } = "default";

        /// <summary>
        /// 数据库配置列表
        /// </summary>
        [XmlElement("database")]
        [JsonProperty("databases")]
        public DatabaseItemConfig[] Items { get; set; }

        /// <summary>
        /// 根据指定名称的本地缓存配置项
        /// </summary>
        public DatabaseItemConfig GetByName(string name)
        {
            return Items?.FirstOrDefault(u => u.Name != null && u.Name.Equals(name));
        }
    }

    /// <summary>
    /// 数据库配置项
    /// </summary>
    public class DatabaseItemConfig
    {
        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 数据库连接配置
        /// </summary>
        [XmlElement("config")]
        [JsonProperty("configs")]
        public DatabaseConnectionConfig[] Configs { get; set; }
    }

    /// <summary>
    /// 数据库主连接配置
    /// </summary>
    public class DatabaseConnectionConfig
    {
        /// <summary>
        /// 配置ID
        /// </summary>
        [XmlAttribute("id")]
        [JsonProperty("id")]
        public string ConfigId { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        [XmlElement("dbType")]
        [JsonProperty("dbType")]
        public DbType DbType { get; set; }

        /// <summary>
        /// 主库连接字符串
        /// </summary>
        [XmlElement("connectionString")]
        [JsonProperty("connectionString")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// 自动关闭连接
        /// </summary>
        [XmlElement("isAutoCloseConnection")]
        [JsonProperty("isAutoCloseConnection")]
        public bool IsAutoCloseConnection { get; set; } = true;

        /// <summary>
        /// 相同线程共享
        /// </summary>
        [XmlElement("isShardSameThread")]
        [JsonProperty("isShardSameThread")]
        public bool IsShardSameThread { get; set; } = false;

        /// <summary>
        /// 从库配置
        /// </summary>
        [XmlElement("slave")]
        [JsonProperty("slaves")]
        public DatabaseSlaveConnectionConfig[] Slaves { get; set; }

        /// <summary>
        /// 扩展配置
        /// </summary>
        [XmlElement("moreSetting")]
        [JsonProperty("moreSetting")]
        public DatabaseConnectionMoreSetting MoreSetting { get; set; }
    }

    /// <summary>
    /// 数据库从连接配置
    /// </summary>
    public class DatabaseSlaveConnectionConfig
    {
        /// <summary>
        /// 命中率
        /// </summary>
        [XmlAttribute("hitRate")]
        [JsonProperty("hitRate")]
        public int HitRate { get; set; } = 1;

        /// <summary>
        /// 从库连接字符串
        /// </summary>
        [XmlElement("connectionString")]
        [JsonProperty("connectionString")]
        public string ConnectionString { get; set; }
    }

    /// <summary>
    /// 数据库连接额外配置
    /// </summary>
    public class DatabaseConnectionMoreSetting
    {
        public bool PgSqlIsAutoToLower { get; set; } = true;

        public bool IsAutoRemoveDataCache { get; set; } = true;

        public bool IsWithNoLockQuery { get; set; } = true;

        public bool DisableNvarchar { get; set; } = true;

        public int DefaultCacheDurationInSeconds { get; set; } = 60;
    }
}
