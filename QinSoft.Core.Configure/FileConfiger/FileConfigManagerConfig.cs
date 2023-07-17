using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace QinSoft.Core.Configure.FileConfiger
{
    /// <summary>
    /// 文件配置器配置管理
    /// </summary>
    [XmlRoot("configManager", Namespace = "http://www.qinsoft.com")]
    public class FileConfigManagerConfig
    {
        [XmlElement("config")]
        public FileConfigItemConfig[] Items { get; set; }

        /// <summary>
        /// 根据指定名称的本地缓存配置项
        /// </summary>
        public FileConfigItemConfig GetByName(string name)
        {
            return Items?.FirstOrDefault(u => u.Name != null && u.Name.Equals(name));
        }
    }

    /// <summary>
    /// 文件配置器配置项
    /// </summary>
    public class FileConfigItemConfig
    {
        /// <summary>
        /// 配置名称
        /// </summary>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// 配置路径
        /// </summary>
        [XmlAttribute("path")]
        public string Path { get; set; }
    }
}
