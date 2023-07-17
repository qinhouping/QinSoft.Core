using System;
using System.Globalization;
using System.Net;
using System.Text;
using QinSoft.Core.Log.NjLog.Manages;
using log4net.Appender;
using log4net.Core;
using log4net.Util;

namespace QinSoft.Core.Log.NjLog.Appender
{
    // Token: 0x0200000A RID: 10
    public class TcpAppender : AppenderSkeleton
    {
        // Token: 0x17000011 RID: 17
        // (get) Token: 0x06000042 RID: 66 RVA: 0x0000301C File Offset: 0x0000121C
        // (set) Token: 0x06000043 RID: 67 RVA: 0x00003034 File Offset: 0x00001234
        public IPAddress RemoteAddress
        {
            get
            {
                return this.m_remoteAddress;
            }
            set
            {
                this.m_remoteAddress = value;
            }
        }

        // Token: 0x17000012 RID: 18
        // (get) Token: 0x06000044 RID: 68 RVA: 0x00003040 File Offset: 0x00001240
        // (set) Token: 0x06000045 RID: 69 RVA: 0x00003058 File Offset: 0x00001258
        public int MemorySize
        {
            get
            {
                return this.m_memorySize;
            }
            set
            {
                this.m_memorySize = value;
            }
        }

        // Token: 0x17000013 RID: 19
        // (get) Token: 0x06000046 RID: 70 RVA: 0x00003064 File Offset: 0x00001264
        // (set) Token: 0x06000047 RID: 71 RVA: 0x0000307C File Offset: 0x0000127C
        public int RemotePort
        {
            get
            {
                return this.m_remotePort;
            }
            set
            {
                bool flag = value < 0 || value > 65535;
                if (flag)
                {
                    throw SystemInfo.CreateArgumentOutOfRangeException("value", value, string.Concat(new string[]
                    {
                        "The value specified is less than ",
                        0.ToString(NumberFormatInfo.InvariantInfo),
                        " or greater than ",
                        65535.ToString(NumberFormatInfo.InvariantInfo),
                        "."
                    }));
                }
                this.m_remotePort = value;
            }
        }

        // Token: 0x17000014 RID: 20
        // (get) Token: 0x06000048 RID: 72 RVA: 0x00003104 File Offset: 0x00001304
        // (set) Token: 0x06000049 RID: 73 RVA: 0x0000311C File Offset: 0x0000131C
        public int LocalPort
        {
            get
            {
                return this.m_localPort;
            }
            set
            {
                bool flag = value != 0 && (value < 0 || value > 65535);
                if (flag)
                {
                    throw SystemInfo.CreateArgumentOutOfRangeException("value", value, string.Concat(new string[]
                    {
                        "The value specified is less than ",
                        0.ToString(NumberFormatInfo.InvariantInfo),
                        " or greater than ",
                        65535.ToString(NumberFormatInfo.InvariantInfo),
                        "."
                    }));
                }
                this.m_localPort = value;
            }
        }

        // Token: 0x17000015 RID: 21
        // (get) Token: 0x0600004A RID: 74 RVA: 0x000031A8 File Offset: 0x000013A8
        // (set) Token: 0x0600004B RID: 75 RVA: 0x000031C0 File Offset: 0x000013C0
        public Encoding Encoding
        {
            get
            {
                return this.m_encoding;
            }
            set
            {
                this.m_encoding = value;
            }
        }

        // Token: 0x17000016 RID: 22
        // (get) Token: 0x0600004C RID: 76 RVA: 0x000031CC File Offset: 0x000013CC
        // (set) Token: 0x0600004D RID: 77 RVA: 0x000031E4 File Offset: 0x000013E4
        protected IPEndPoint RemoteEndPoint
        {
            get
            {
                return this.m_remoteEndPoint;
            }
            set
            {
                this.m_remoteEndPoint = value;
            }
        }

        // Token: 0x17000017 RID: 23
        // (get) Token: 0x0600004E RID: 78 RVA: 0x000031F0 File Offset: 0x000013F0
        // (set) Token: 0x0600004F RID: 79 RVA: 0x00003208 File Offset: 0x00001408
        protected int ConnInterval
        {
            get
            {
                return this.m_connInterval;
            }
            set
            {
                this.m_connInterval = value;
            }
        }

        // Token: 0x06000050 RID: 80 RVA: 0x00003214 File Offset: 0x00001414
        public override void ActivateOptions()
        {
            base.ActivateOptions();
            bool flag = this.RemoteAddress == null;
            if (flag)
            {
                throw new ArgumentNullException("The required property 'Address' was not specified.");
            }
            bool flag2 = this.RemotePort < 0 || this.RemotePort > 65535;
            if (flag2)
            {
                throw SystemInfo.CreateArgumentOutOfRangeException("this.RemotePort", this.RemotePort, string.Concat(new string[]
                {
                    "The RemotePort is less than ",
                    0.ToString(NumberFormatInfo.InvariantInfo),
                    " or greater than ",
                    65535.ToString(NumberFormatInfo.InvariantInfo),
                    "."
                }));
            }
            bool flag3 = this.LocalPort != 0 && (this.LocalPort < 0 || this.LocalPort > 65535);
            if (flag3)
            {
                throw SystemInfo.CreateArgumentOutOfRangeException("this.LocalPort", this.LocalPort, string.Concat(new string[]
                {
                    "The LocalPort is less than ",
                    0.ToString(NumberFormatInfo.InvariantInfo),
                    " or greater than ",
                    65535.ToString(NumberFormatInfo.InvariantInfo),
                    "."
                }));
            }
            this.RemoteEndPoint = new IPEndPoint(this.RemoteAddress, this.RemotePort);
            bool flag4 = this.MemorySize > 2048;
            if (flag4)
            {
                this.m_memorySize = 2048;
            }
            bool flag5 = this.MemorySize <= 0;
            if (flag5)
            {
                this.m_memorySize = 0;
            }
            bool flag6 = this.ConnInterval <= 0;
            if (flag6)
            {
                this.ConnInterval = 15;
            }
            this.appenderId = Guid.NewGuid().ToString();
            this.engineManager = TcpLogEventEngineManager.Instance();
            this.engine = this.engineManager.StartEngine(this.RemoteAddress, this.RemotePort, this.appenderId, this.MemorySize, this.ConnInterval);
        }

        // Token: 0x06000051 RID: 81 RVA: 0x00003408 File Offset: 0x00001608
        protected override void Append(LoggingEvent loggingEvent)
        {
            try
            {
                bool flag = this.engine == null;
                if (flag)
                {
                    this.engine = this.engineManager.StartEngine(this.RemoteAddress, this.RemotePort, this.appenderId, this.MemorySize, this.ConnInterval);
                }
                byte[] bytes = this.m_encoding.GetBytes(base.RenderLoggingEvent(loggingEvent));
                bool flag2 = this.engine.Send(bytes);
                bool flag3 = !flag2;
                if (flag3)
                {
                    this.MemoryAppend(loggingEvent);
                }
            }
            catch (Exception ex)
            {
                this.MemoryAppend(loggingEvent);
                this.ErrorHandler.Error(string.Concat(new object[]
                {
                    "Unable to send logging event to remote host ",
                    this.RemoteAddress.ToString(),
                    " on port ",
                    this.RemotePort,
                    "."
                }), ex, ErrorCode.WriteFailure);
            }
        }

        // Token: 0x06000052 RID: 82 RVA: 0x000034F4 File Offset: 0x000016F4
        protected void MemoryAppend(LoggingEvent loggingEvent)
        {
            bool flag = this.MemorySize <= 0;
            if (!flag)
            {
                object syncRoot = this.engine.eventsList.SyncRoot;
                lock (syncRoot)
                {
                    bool flag3 = this.engine.eventsList.Count >= this.MemorySize;
                    if (flag3)
                    {
                        this.engine.eventsList.RemoveAt(0);
                    }
                    byte[] bytes = this.m_encoding.GetBytes(base.RenderLoggingEvent(loggingEvent));
                    this.engine.eventsList.Add(bytes);
                }
            }
        }

        // Token: 0x17000018 RID: 24
        // (get) Token: 0x06000053 RID: 83 RVA: 0x000035AC File Offset: 0x000017AC
        protected override bool RequiresLayout
        {
            get
            {
                return true;
            }
        }

        // Token: 0x06000054 RID: 84 RVA: 0x000035C0 File Offset: 0x000017C0
        protected override void OnClose()
        {
            base.OnClose();
            bool flag = this.engineManager != null;
            if (flag)
            {
                this.engineManager.RemoveEngine(this.RemoteAddress, this.RemotePort, this.appenderId);
            }
        }

        // Token: 0x0400002B RID: 43
        private IPAddress m_remoteAddress;

        // Token: 0x0400002C RID: 44
        private int m_remotePort;

        // Token: 0x0400002D RID: 45
        private IPEndPoint m_remoteEndPoint;

        // Token: 0x0400002E RID: 46
        private int m_localPort;

        // Token: 0x0400002F RID: 47
        private TcpLogEventEngine engine;

        // Token: 0x04000030 RID: 48
        private TcpLogEventEngineManager engineManager;

        // Token: 0x04000031 RID: 49
        private string appenderId = "";

        // Token: 0x04000032 RID: 50
        private int m_connInterval;

        // Token: 0x04000033 RID: 51
        private int m_memorySize;

        // Token: 0x04000034 RID: 52
        private Encoding m_encoding = Encoding.Default;
    }
}
