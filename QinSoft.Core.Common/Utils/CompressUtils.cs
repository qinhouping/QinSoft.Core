using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Text;

namespace QinSoft.Core.Common.Utils
{
    public static class CompressUtils
    {
        public static byte[] Compress(this byte[] data, CompressionLevel level = CompressionLevel.Optimal)
        {
            ObjectUtils.CheckNull(data, nameof(data));
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (GZipStream gzipStream = new GZipStream(memoryStream, level, true))
                {
                    gzipStream.Write(data, 0, data.Length);
                }

                return memoryStream.ToArray();
            }
        }

        public static Stream Compress(this Stream stream, CompressionLevel level = CompressionLevel.Optimal)
        {
            ObjectUtils.CheckNull(stream, paramName: nameof(stream));
            MemoryStream memoryStream = new MemoryStream();
            using (GZipStream gzipStream = new GZipStream(memoryStream, level, true))
            {
                stream.CopyTo(gzipStream);
            }
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
        }

        public static byte[] Decompress(this byte[] data)
        {
            ObjectUtils.CheckNull(data, nameof(data));
            using (MemoryStream memoryStream = new MemoryStream(data))
            {
                using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress, true))
                {
                    return gzipStream.ToBytes();
                }
            }
        }
    }
}
