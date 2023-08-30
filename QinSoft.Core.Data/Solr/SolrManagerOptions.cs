using QinSoft.Core.Configure;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Data.Solr
{
    /// <summary>
    /// Solr管理选项
    /// </summary>
    public class SolrManagerOptions
    {
        /// <summary>
        /// 配置名称，默认"SolrManagerConfig"
        /// </summary>
        public string ConfigName { get; set; } = "SolrManagerConfig";

        /// <summary>
        /// 配置格式，默认"ConfigFormat.XML"
        /// </summary>
        public ConfigFormat ConfigFormat { get; set; } = ConfigFormat.XML;
    }
}
