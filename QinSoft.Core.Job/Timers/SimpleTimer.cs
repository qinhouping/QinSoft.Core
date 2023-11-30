using QinSoft.Core.Common.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Job.Timers
{
    public class SimpleTimer : TimerBase
    {
        public new double Interval { get; protected set; }

        public SimpleTimer(double interval)
        {
            ObjectUtils.CheckRange(interval, 0, Double.MaxValue, nameof(interval));
            this.Interval = interval;
        }

        public override double NextInterval()
        {
            return this.Interval;
        }
    }
}
