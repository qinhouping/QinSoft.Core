using Cronos;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Job.Timers
{
    public class CronTimer : TimerBase
    {
        public CronExpression CronExpression { get; protected set; }
        public CronTimer(string cronExpression)
        {
            this.CronExpression = CronExpression.Parse(cronExpression, CronFormat.IncludeSeconds);
        }

        public override double NextInterval()
        {
            DateTime now = DateTime.Now;
            now = DateTime.SpecifyKind(now, DateTimeKind.Utc);
            DateTime? next = CronExpression.GetNextOccurrence(now);
            if (next == null) return 0;
            return (next - now).Value.TotalMilliseconds;
        }
    }
}
