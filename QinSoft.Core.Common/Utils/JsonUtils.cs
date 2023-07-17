using System;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace QinSoft.Core.Common.Utils
{
    /// <summary>
    /// Json工具类
    /// </summary>
    public static class JsonUtils
    {
        /// <summary>
        /// Json默认时间转换器
        /// </summary>
        public static DateTimeConverterBase DefaultDateTimeConvert { get; set; } = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };

        /// <summary>
        /// JSON默认日志
        /// </summary>
        public static ILog DefaultLog { get; set; } = LogManager.GetLogger(typeof(JsonUtils));

        /// <summary>
        /// JSON序列化
        /// </summary>
        public static string ToJson<T>(this T obj)
        {
            if (null == obj)
            {
                DefaultLog?.Debug("to json: null");
                return null;
            }
            else
            {
                try
                {
                    string res = JsonConvert.SerializeObject(obj, DefaultDateTimeConvert);
                    DefaultLog?.Debug(string.Format("to json:{0}", res));
                    return res;
                }
                catch (Exception e)
                {
                    DefaultLog?.Debug("to json", e);
                    throw e;
                }
            }
        }

        /// <summary>
        /// JSON反序列化
        /// </summary>
        public static T FromJson<T>(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                DefaultLog?.Debug(string.Format("from json:{0}", null));
                return default;
            }
            else
            {
                try
                {
                    DefaultLog?.Debug(string.Format("from json:{0}", value));
                    return JsonConvert.DeserializeObject<T>(value);
                }
                catch (Exception e)
                {
                    DefaultLog?.Debug("from json", e);
                    throw e;
                }
            }
        }
    }
}
