using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Google.Protobuf;

/// <summary>
/// protobuf工具类
/// </summary>
namespace QinSoft.Core.Common.Utils
{
    public static class ProtobufUtils
    {
        public static byte[] ToProtobuf<T>(this T obj) where T : IMessage
        {
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
            var message = new T();
            using (var stream = new CodedInputStream(bytes))
            {
                message.MergeFrom(stream);
            }
            return message;
        }
    }
}
