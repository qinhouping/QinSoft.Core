using QinSoft.Core.Configure;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Data.Database
{
    /// <summary>
    /// 数据库管理选项
    /// </summary>
    public class DatabaseManagerOptions
    {
        /// <summary>
        /// 配置名称，默认"DatabaseManagerConfig"
        /// </summary>
        public string ConfigName { get; set; } = "DatabaseManagerConfig";

        /// <summary>
        /// 配置格式，默认"ConfigFormat.XML"
        /// </summary>
        public ConfigFormat ConfigFormat { get; set; } = ConfigFormat.XML;
    }
}
