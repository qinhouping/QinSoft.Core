using QinSoft.Core.Job.Timers;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Job
{
    /// <summary>
    /// Job上下文
    /// </summary>
    public class JobContext
    {
        public TimerBase JobTimer { get; private set; }

        public object JobParam { get; private set; }

        public long Tally { get; private set; }

        public JobContext(TimerBase jobTimer, object jbParam, long tally)
        {
            this.JobTimer = jobTimer;
            this.JobParam = jbParam;
            this.Tally= tally;
        }
    }
}
