using System;
using System.Globalization;
using System.IO;
using QinSoft.Core.Common.Utils;
using QinSoft.Core.Log.NjLog.Models;
using QinSoft.Core.Log.NjLog.Util;
using log4net.Core;
using log4net.Layout;

namespace QinSoft.Core.Log.NjLog.Layout
{
    // Token: 0x02000007 RID: 7
    public class NjLogLayout : LayoutSkeleton
    {
        // Token: 0x1700000E RID: 14
        // (get) Token: 0x06000033 RID: 51 RVA: 0x00002E6A File Offset: 0x0000106A
        // (set) Token: 0x06000034 RID: 52 RVA: 0x00002E72 File Offset: 0x00001072
        public string projectCode { get; set; }

        // Token: 0x1700000F RID: 15
        // (get) Token: 0x06000035 RID: 53 RVA: 0x00002E7B File Offset: 0x0000107B
        // (set) Token: 0x06000036 RID: 54 RVA: 0x00002E83 File Offset: 0x00001083
        public string appCode { get; set; }

        // Token: 0x17000010 RID: 16
        // (get) Token: 0x06000037 RID: 55 RVA: 0x00002E8C File Offset: 0x0000108C
        // (set) Token: 0x06000038 RID: 56 RVA: 0x00002E94 File Offset: 0x00001094
        public string logVer { get; set; }

        // Token: 0x0600003A RID: 58 RVA: 0x00002E9D File Offset: 0x0000109D
        public override void ActivateOptions()
        {
        }

        // Token: 0x0600003B RID: 59 RVA: 0x00002EA0 File Offset: 0x000010A0
        public override void Format(TextWriter writer, LoggingEvent loggingEvent)
        {
            NjLogLayoutModel obj = new NjLogLayoutModel
            {
                datetime = loggingEvent.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss.fff", DateTimeFormatInfo.InvariantInfo),
                logger = loggingEvent.LocationInformation.ClassName,
                msg = loggingEvent.RenderedMessage + (string.IsNullOrEmpty(loggingEvent.GetExceptionString()) ? "" : loggingEvent.GetExceptionString()),
                thread = loggingEvent.ThreadName,
                level = loggingEvent.Level.DisplayName,
                projectCode = this.projectCode,
                appCode = this.appCode,
                host = IpHelper.ip,
                logId = IdWorker.nextId().ToString().PadLeft(20, '0'),
                logVer = this.logVer,
                extend = ""
            };
            string value = obj.ToJson();
            writer.Write(value);
            writer.Write("\n");
        }

        // Token: 0x0400002A RID: 42
        private const string PREFIX = "log4net";
    }
}
