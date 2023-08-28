using QinSoft.Core.Configure;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Data.Zookeeper
{
    /// <summary>
    /// zookeeper管理选项
    /// </summary>
    public class ZookeeperManagerOptions
    {
        /// <summary>
        /// 配置名称，默认"ZookeeperManagerConfig"
        /// </summary>
        public string ConfigName { get; set; } = "ZookeeperManagerConfig";

        /// <summary>
        /// 配置格式，默认"ConfigFormat.XML"
        /// </summary>
        public ConfigFormat ConfigFormat { get; set; } = ConfigFormat.XML;
    }
}
