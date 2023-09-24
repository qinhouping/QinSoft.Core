using QinSoft.Core.Configure;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Data.Influx
{
    /// <summary>
    /// Influx管理选项
    /// </summary>
    public class InfluxManagerOptions
    {
        /// <summary>
        /// 配置名称，默认"InfluxManagerConfig"
        /// </summary>
        public string ConfigName { get; set; } = "InfluxManagerConfig";

        /// <summary>
        /// 配置格式，默认"ConfigFormat.XML"
        /// </summary>
        public ConfigFormat ConfigFormat { get; set; } = ConfigFormat.XML;
    }
}
