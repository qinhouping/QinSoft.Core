using Microsoft.Extensions.Logging;
using QinSoft.Core.Common.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace QinSoft.Core.EventBus
{
    public abstract class Channel : IDisposable
    {
        protected ILogger logger { get; set; }

        /// <summary>
        /// Write队列
        /// </summary>
        protected ConcurrentQueue<ChannelData> WriteQueue { get; set; }

        protected Thread WriteQueueThread { get; set; }

        /// <summary>
        /// Read队列
        /// </summary>
        protected ConcurrentQueue<ChannelData> ReadQueue { get; set; }

        protected Thread ReadQueueThread { get; set; }

        protected CancellationTokenSource QueueThreadCancelToken { get; set; }

        public Channel(ILogger logger)
        {
            this.logger = logger;
            this.QueueThreadCancelToken = new CancellationTokenSource();

            this.WriteQueue = new ConcurrentQueue<ChannelData>();
            this.WriteQueueThread = new Thread(() =>
            {
                var token = QueueThreadCancelToken.Token;
                while (!token.IsCancellationRequested)
                {
                    ChannelData data = null;
                    if (WriteQueue.TryDequeue(out data))
                    {
                        try
                        {
                            WriteCore(data);
                        }
                        catch (Exception e)
                        {
                            this.logger.LogError(e, "Channel WriteCode:" + e.Message);
                        }
                    }
                    Thread.Sleep(100);
                }
            });
            this.WriteQueueThread.IsBackground = true;
            this.WriteQueueThread.Start();

            this.ReadQueue = new ConcurrentQueue<ChannelData>();
            this.ReadQueueThread = new Thread(() =>
            {
                var token = QueueThreadCancelToken.Token;
                while (!token.IsCancellationRequested)
                {
                    ChannelData data = null;
                    if (ReadQueue.TryDequeue(out data))
                    {
                        try
                        {
                            ReadCore(data);
                        }
                        catch (Exception e)
                        {
                            this.logger.LogError(e, "Channel ReadCore:" + e.Message);
                        }
                    }
                    Thread.Sleep(100);
                }
            });
            this.ReadQueueThread.IsBackground = true;
            this.ReadQueueThread.Start();
        }

        public virtual bool Write(ChannelData data)
        {
            if (data == null) return false;
            //写入内部队列
            this.WriteQueue.Enqueue(data);
            this.logger?.LogDebug($"Channel WriteQueue Enqueue Data: {JsonUtils.ToJson(data)}");
            return true;
        }

        protected abstract void WriteCore(ChannelData data);


        public event EventHandler<ChannelData> Readed;

        public virtual bool Read(ChannelData data)
        {
            if (data == null) return false;
            //写入内部队列
            this.ReadQueue.Enqueue(data);
            this.logger?.LogDebug($"Channel ReadQueue Enqueue Data: {JsonUtils.ToJson(data)}");
            return true;
        }

        protected virtual void ReadCore(ChannelData data)
        {
            this.Readed?.Invoke(this, data);
        }

        public virtual void Dispose()
        {
            this.QueueThreadCancelToken?.Cancel();
        }
    }
}
