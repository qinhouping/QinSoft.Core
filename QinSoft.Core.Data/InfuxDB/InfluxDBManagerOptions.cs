using QinSoft.Core.Configure;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Data.InfluxDB
{
    /// <summary>
    /// Influx管理选项
    /// </summary>
    public class InfluxDBManagerOptions
    {
        /// <summary>
        /// 配置名称，默认"InfluxDBManagerConfig"
        /// </summary>
        public string ConfigName { get; set; } = "InfluxDBManagerConfig";

        /// <summary>
        /// 配置格式，默认"ConfigFormat.XML"
        /// </summary>
        public ConfigFormat ConfigFormat { get; set; } = ConfigFormat.XML;
    }
}
