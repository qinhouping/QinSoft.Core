using QinSoft.Core.Configure;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.MQ.RabbitMQ
{
    /// <summary>
    /// RabbitMQ管理选项
    /// </summary>
    public class RabbitMQManagerOptions
    {
        /// <summary>
        /// 配置名称，默认"RabbitMQManagerConfig"
        /// </summary>
        public string ConfigName { get; set; } = "RabbitMQManagerConfig";

        /// <summary>
        /// 配置格式，默认"ConfigFormat.XML"
        /// </summary>
        public ConfigFormat ConfigFormat { get; set; } = ConfigFormat.XML;
    }
}
