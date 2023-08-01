using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace QinSoft.Core.Common.Utils
{
    /// <summary>
    /// Xml工具类
    /// </summary>
    public static class XmlUtils
    {
        /// <summary>
        /// XML默认日志
        /// </summary>
        public static ILog DefaultLog { get; set; } = LogManager.GetLogger(typeof(XmlUtils));

        /// <summary>
        /// XML序列化
        /// </summary>
        public static string ToXml(this object obj)
        {
            if (null == obj)
            {
                DefaultLog?.Debug("to xml: null");
                return null;
            }
            else
            {
                try
                {
                    using (StreamReader stream = new StreamReader(new MemoryStream()))
                    {
                        XmlSerializer serializer = new XmlSerializer(obj.GetType());
                        serializer.Serialize(stream.BaseStream, obj);
                        stream.BaseStream.Position = 0;
                        string res = stream.ReadToEnd();
                        DefaultLog?.Debug(string.Format("to xml:{0}", res));
                        return res;
                    }
                }
                catch (Exception e)
                {
                    DefaultLog?.Debug("to xml", e);
                    throw e;
                }
            }

        }

        /// <summary>
        /// XML序列化
        /// </summary>
        public static async Task<string> ToXmlAsync(this object obj)
        {
            return await ExecuteUtils.ExecuteInTask(ToXml, obj);
        }

        /// <summary>
        /// XML反序列化
        /// </summary>
        public static T FromXml<T>(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                DefaultLog?.Debug(string.Format("from xml:{0}", null));
                return default;
            }
            else
            {
                try
                {
                    DefaultLog?.Debug(string.Format("from xml:{0}", value));
                    using (StringReader sr = new StringReader(value))
                    {
                        XmlSerializer xmldes = new XmlSerializer(typeof(T));
                        return (T)xmldes.Deserialize(sr);
                    }
                }
                catch (Exception e)
                {
                    DefaultLog.Debug("from xml", e);
                    throw e;
                }
            }

        }

        /// <summary>
        /// XML反序列化
        /// </summary>
        public static async Task<T> FromXmlAsync<T>(this string value)
        {
            return await ExecuteUtils.ExecuteInTask(FromXml<T>, value);
        }
    }
}
