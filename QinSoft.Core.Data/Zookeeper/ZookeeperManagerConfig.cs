using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace QinSoft.Core.Data.Zookeeper
{
    /// <summary>
    /// Zookeeper管理器配置
    /// </summary>
    [XmlRoot("zookeeperManager", Namespace = "http://www.qinsoft.com")]
    public class ZookeeperManagerConfig
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
        [XmlElement("zookeeper")]
        [JsonProperty("zookeepers")]
        public ZookeeperItemConfig[] Items { get; set; }

        /// <summary>
        /// 根据指定名称的Zookeeper配置项
        /// </summary>
        public ZookeeperItemConfig GetByName(string name)
        {
            return Items?.FirstOrDefault(u => u.Name != null && u.Name.Equals(name));
        }
    }

    /// <summary>
    /// Zookeeper配置项
    /// </summary>
    public class ZookeeperItemConfig
    {
        /// <summary>
        /// 名称
        /// </summary>
        [XmlAttribute("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Zookeeper连接配置
        /// </summary>
        [XmlElement("connectionString")]
        [JsonProperty("connectionString")]
        public string ConnectionString { get; set; }

        /// <summary>
        /// Zookeeper超时
        /// </summary>
        [XmlElement("sessionTimeout")]
        [JsonProperty("sessionTimeout")]
        public int SessionTimeout { get; set; } = 10000;

        /// <summary>
        /// Zookeeper连接配置
        /// </summary>
        [XmlElement("authInfo")]
        [JsonProperty("authInfos")]
        public ZookeeperAuthConfig[] AuthInfos { get; set; }
    }

    /// <summary>
    /// Zookeeper认证配置
    /// </summary>
    public class ZookeeperAuthConfig
    {
        [XmlElement("schema")]
        [JsonProperty("schema")]
        public string Schema { get; set; }

        [XmlElement("auth")]
        [JsonProperty("auth")]
        public string Auth { get; set; }
    }
}
