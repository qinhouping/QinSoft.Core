using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.EventBus
{
    public class ChannelData
    {
        public string Id { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string Topic { get; set; }

        /// <summary>
        /// 请求头
        /// </summary>
        public IDictionary<string, string> Headers { get; set; }

        /// <summary>
        ///请求体
        /// </summary>
        public byte[] Payload { get; set; }
    }
}
