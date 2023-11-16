using QinSoft.Core.Configure;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.MQ.MQTT
{
    /// <summary>
    /// kafka管理选项
    /// </summary>
    public class MQTTManagerOptions
    {
        /// <summary>
        /// 配置名称，默认"MQTTManagerConfig"
        /// </summary>
        public string ConfigName { get; set; } = "MQTTManagerConfig";

        /// <summary>
        /// 配置格式，默认"ConfigFormat.XML"
        /// </summary>
        public ConfigFormat ConfigFormat { get; set; } = ConfigFormat.XML;
    }
}
