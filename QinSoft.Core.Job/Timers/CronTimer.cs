using Cronos;
using QinSoft.Core.Common.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Job.Timers
{
    public class CronTimer : TimerBase
    {
        public string CronExpress { get; protected set; }

        public CronExpression CronExpression { get; protected set; }
        public CronTimer(string cronExpression)
        {
            ObjectUtils.CheckEmpty(cronExpression, nameof(cronExpression));
            this.CronExpress = cronExpression;
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
