using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Configure.FileConfiger
{
    /// <summary>
    /// 文件配置器配置选项
    /// </summary>
    public class FileConfigerOptions
    {
        /// <summary>
        /// 配置目录名称，默认"Configs" 
        /// </summary>
        public string ConfigDirectoryName { get; set; } = "Configs";

        /// <summary>
        /// 配置管理文件名称，默认"ConfigManager"
        /// </summary>
        public string ConfigManagerFileName { get; set; } = "ConfigManager";

        /// <summary>
        /// 配置内容缓存时间，单位秒，默认"0"
        /// 0：永久
        /// </summary>
        public ulong ExpireIn { get; set; } = 0;

        /// <summary>
        /// 配置管理文件格式，默认"ConfigFormat.XML"
        /// </summary>
        public ConfigFormat ConfigManagerFileFormat { get; set; } = ConfigFormat.XML;
    }
}
