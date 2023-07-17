using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace QinSoft.Core.Common.Utils
{
    /// <summary>
    /// 文件工具类
    /// </summary>
    public static class FileUtils
    {

        /// <summary>
        /// 创建文件所在目录
        /// </summary>
        public static void CreateFileDirectory(this FileInfo file)
        {
            DirectoryInfo directory = file.Directory;
            if (!directory.Exists)
            {
                directory.Create();
            }
        }

        /// <summary>
        /// 获取目录下的文件
        /// 正则匹配
        /// </summary>
        public static List<FileInfo> GetFiles(this DirectoryInfo directory, string searchPattern, bool fetch)
        {
            List<FileInfo> files = new List<FileInfo>();
            if (directory.Exists)
            {
                files.AddRange(directory.GetFiles().Where(f =>
                {
                    //忽律大小写
                    return Regex.IsMatch(f.Name, searchPattern, RegexOptions.IgnoreCase);
                }));
                if (fetch)
                {
                    foreach (DirectoryInfo subDirectory in directory.GetDirectories())
                    {
                        files.AddRange(subDirectory.GetFiles(searchPattern, fetch));
                    }
                }
            }
            return files;
        }
    }
}
