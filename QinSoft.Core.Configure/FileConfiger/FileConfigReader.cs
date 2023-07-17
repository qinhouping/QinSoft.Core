using QinSoft.Core.Common.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace QinSoft.Core.Configure.FileConfiger
{
    /// <summary>
    /// 文件配置读取器
    /// </summary>
    public class FileConfigReader
    {
        /// <summary>
        /// 默认编码
        /// </summary>
        public Encoding DefaultEncoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// 获取文件内容，并反序列化
        /// </summary>
        public virtual T GetFileContent<T>(FileInfo file, ConfigFormat format)
        {
            if (file == null || !file.Exists)
            {
                return default;
            }

            string content = File.ReadAllText(file.ToString(), DefaultEncoding);

            switch (format)
            {
                case ConfigFormat.JSON: return content.FromJson<T>();
                case ConfigFormat.XML: return content.FromXml<T>();
                default:
                    throw new ConfigureException(string.Format("not support config format:{0}", format));
            }
        }

        /// <summary>
        /// 获取支持的文件类型
        /// </summary>
        public virtual string[] GetSupportFileExtensions()
        {
            return new string[] { "json", "xml", "config" };
        }
    }
}
