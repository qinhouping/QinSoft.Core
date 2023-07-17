using QinSoft.Core.Configure;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Data.MongoDB
{
    /// <summary>
    /// mongodb管理选项
    /// </summary>
    public class MongoDBManagerOptions
    {
        /// <summary>
        /// 配置名称，默认"MongoDBManagerConfig"
        /// </summary>
        public string ConfigName { get; set; } = "MongoDBManagerConfig";

        /// <summary>
        /// 配置格式，默认"ConfigFormat.XML"
        /// </summary>
        public ConfigFormat ConfigFormat { get; set; } = ConfigFormat.XML;
    }
}
