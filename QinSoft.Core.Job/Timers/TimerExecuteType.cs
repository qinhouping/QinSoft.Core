using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Job.Timers
{
    public enum TimerExecuteType
    {
        /// <summary>
        /// 并行执行
        /// </summary>
        Parallel,
        /// <summary>
        /// 串行执行
        /// </summary>
        Serial
    }
}
