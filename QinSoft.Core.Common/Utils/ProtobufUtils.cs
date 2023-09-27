using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Protobuf;
using log4net;

/// <summary>
/// protobuf工具类
/// </summary>
namespace QinSoft.Core.Common.Utils
{
    public static class ProtobufUtils
    {
        /// <summary>
        /// Protobuf默认日志
        /// </summary>
        public static ILog DefaultLog { get; set; } = LogManager.GetLogger(typeof(ProtobufUtils));

        public static byte[] ToProtobuf<T>(this T obj) where T : IMessage
        {
            if (null == obj)
            {
                DefaultLog?.Debug("to protobuf: null");
                return null;
            }
            using (MemoryStream ms = new MemoryStream()) {
                using (CodedOutputStream stream = new CodedOutputStream(ms))
                {
                    obj.WriteTo(stream);
                }
                return ms.ToArray();
            }
        }

        public static T FromProtobuf<T>(byte[] bytes) where T : IMessage,new()
        {
            if (null == bytes)
            {
                DefaultLog?.Debug("from protobuf: null");
                return default;
            }
            
            var message = new T();
            using (var stream = new CodedInputStream(bytes))
            {
                message.MergeFrom(stream);
            }
            return message;
        }
    }
}
