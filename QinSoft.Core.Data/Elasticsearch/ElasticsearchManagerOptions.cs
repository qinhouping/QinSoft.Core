using QinSoft.Core.Configure;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Data.Elasticsearch
{
    /// <summary>
    /// elasticsearch管理选项
    /// </summary>
    public class ElasticsearchManagerOptions
    {
        /// <summary>
        /// 配置名称，默认"ElasticsearchManagerConfig"
        /// </summary>
        public string ConfigName { get; set; } = "ElasticsearchManagerConfig";

        /// <summary>
        /// 配置格式，默认"ConfigFormat.XML"
        /// </summary>
        public ConfigFormat ConfigFormat { get; set; } = ConfigFormat.XML;
    }
}
