
using QinSoft.Core.Common.Utils;
using System.ComponentModel;
using System.Timers;

namespace QinSoft.Core.Job.Timers
{
    /// <summary>
    /// 定时器基类
    /// </summary>
    public abstract class TimerBase
    {
        /// <summary>
        /// 内部定时器
        /// </summary>
        protected Timer InnerTimer { get; set; }

        public TimerBase()
        {
            InnerTimer = new Timer();
            InnerTimer.Elapsed += Timer_Elapsed;
        }

        /// <summary>
        /// 内部定时器时间处理
        /// </summary>
        protected virtual void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            bool autoStart = this.InnerTimer.AutoReset;
            ///停止计时器
            this.Stop();
            if (autoStart && this.ExecuteType == TimerExecuteType.Parallel)
            {
                //重新启动
                this.Start();
            }
            try
            {
                this.Elapsed?.Invoke(this, e);
            }
            catch
            {
            }
            if (autoStart && this.ExecuteType == TimerExecuteType.Serial)
            {
                //重新启动
                this.Start();
            }
        }

        /// <summary>
        /// 下一执行时间，单位毫秒
        /// </summary>
        /// <returns></returns>
        public abstract double NextInterval();

        public virtual bool AutoReset
        {
            get
            {
                return this.InnerTimer.AutoReset;
            }
            set
            {
                this.InnerTimer.AutoReset = value;
            }
        }

        /// <summary>
        /// 执行方式，默认并行执行
        /// </summary>
        public virtual TimerExecuteType ExecuteType { get; set; } = TimerExecuteType.Parallel;

        public virtual bool Enabled
        {
            get
            {
                return this.InnerTimer.Enabled;
            }
        }

        public virtual double Interval
        {
            get
            {
                return this.InnerTimer.Interval;
            }
        }

        public virtual event ElapsedEventHandler Elapsed;

        public virtual void Start()
        {
            this.InnerTimer.Interval = NextInterval();
            this.InnerTimer.Start();
        }

        public virtual void Stop()
        {
            this.InnerTimer.Stop();
        }

        public virtual void Close()
        {
            this.InnerTimer.Close();
        }

        public virtual void Dispose()
        {
            this.InnerTimer.Dispose();
        }

    }
}