using QinSoft.Core.Job.Timers;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Job.Schedulers
{
    /// <summary>
    /// 作业调度器
    /// </summary>
    public interface IScheduler : IDisposable
    {
        bool AddJob(string jobName, TimerBase jobTimer, IJob job, object jobParam = null);

        bool RemoveJob(string jobName);

        bool StartJob(string jobName);

        bool StopJob(string jobName);

        bool ExistsJob(string jobName);
    }
}
