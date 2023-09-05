using Newtonsoft.Json;
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
        public string DbType { get; set; } = "MySql";

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
        [XmlElement("isAutoRemoveDataCache")]
        [JsonProperty("isAutoRemoveDataCache")]
        public bool IsAutoRemoveDataCache
        {
            get;
            set;
        }

        [XmlElement("isWithNoLockQuery")]
        [JsonProperty("isWithNoLockQuery")]
        public bool IsWithNoLockQuery
        {
            get;
            set;
        }

        [XmlElement("isWithNoLockSubquery")]
        [JsonProperty("isWithNoLockSubquery")]
        public bool IsWithNoLockSubquery
        {
            get;
            set;
        }

        [XmlElement("disableNvarchar")]
        [JsonProperty("disableNvarchar")]
        public bool DisableNvarchar
        {
            get;
            set;
        }

        [XmlElement("disableMillisecond")]
        [JsonProperty("disableMillisecond")]
        public bool DisableMillisecond
        {
            get;
            set;
        }

        [XmlElement("pgSqlIsAutoToLower")]
        [JsonProperty("pgSqlIsAutoToLower")]
        public bool PgSqlIsAutoToLower
        {
            get;
            set;
        } = true;

        [XmlElement("pgSqlIsAutoToLowerCodeFirst")]
        [JsonProperty("pgSqlIsAutoToLowerCodeFirst")]
        public bool PgSqlIsAutoToLowerCodeFirst
        {
            get;
            set;
        } = true;

        [XmlElement("isAutoToUpper")]
        [JsonProperty("isAutoToUpper")]
        public bool IsAutoToUpper
        {
            get;
            set;
        } = true;

        [XmlElement("defaultCacheDurationInSeconds")]
        [JsonProperty("defaultCacheDurationInSeconds")]
        public int DefaultCacheDurationInSeconds
        {
            get;
            set;
        }

        [XmlElement("tableEnumIsString")]
        [JsonProperty("tableEnumIsString")]
        public bool? TableEnumIsString
        {
            get;
            set;
        }

        [XmlElement("dbMinDate")]
        [JsonProperty("dbMinDate")]
        public DateTime? DbMinDate
        {
            get;
            set;
        } = Convert.ToDateTime("1900-01-01");

        [XmlElement("isNoReadXmlDescription")]
        [JsonProperty("isNoReadXmlDescription")]
        public bool IsNoReadXmlDescription
        {
            get;
            set;
        }

        [XmlElement("sqlServerCodeFirstNvarchar")]
        [JsonProperty("sqlServerCodeFirstNvarchar")]
        public bool SqlServerCodeFirstNvarchar
        {
            get;
            set;
        }

        [XmlElement("isAutoUpdateQueryFilter")]
        [JsonProperty("isAutoUpdateQueryFilter")]
        public bool IsAutoUpdateQueryFilter
        {
            get;
            set;
        }


        [XmlElement("isAutoDeleteQueryFilter")]
        [JsonProperty("isAutoDeleteQueryFilter")]
        public bool IsAutoDeleteQueryFilter
        {
            get;
            set;
        }

        [XmlElement("enableModelFuncMappingColumn")]
        [JsonProperty("enableModelFuncMappingColumn")]
        public bool EnableModelFuncMappingColumn
        {
            get;
            set;
        }

        [XmlElement("enableOracleIdentity")]
        [JsonProperty("enableOracleIdentity")]
        public bool EnableOracleIdentity
        {
            get;
            set;
        }
    }
}
