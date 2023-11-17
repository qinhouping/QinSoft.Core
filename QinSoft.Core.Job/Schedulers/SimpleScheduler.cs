using QinSoft.Core.Common.Utils;
using QinSoft.Core.Job.Timers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Job.Schedulers
{
    /// <summary>
    /// 简单调度器实现
    /// </summary>
    public class SimpleScheduler : IScheduler
    {
        protected ConcurrentDictionary<string, JobItem> JobItems { get; set; }
        public SimpleScheduler()
        {
            this.JobItems = new ConcurrentDictionary<string, JobItem>();
        }

        protected virtual void BeginSchedule(JobItem item)
        {
            item.JobTimer.Start();
            item.Status = SchedulerStatusEnum.Scheduling;
        }

        protected virtual void EndSchedule(JobItem item)
        {
            item.JobTimer.Stop();
            item.Status = SchedulerStatusEnum.Unscheduled;
        }

        protected virtual void DisposeSchedule(JobItem item)
        {
            item.JobTimer.Dispose();
        }

        public virtual bool AddJob(string jobName, TimerBase jobTimer, IJob job, object jobParam = null)
        {
            ObjectUtils.CheckEmpty(jobName, "jobName");
            ObjectUtils.CheckNull(jobTimer, "jobTimer");
            ObjectUtils.CheckNull(job, "job");

            JobItem item = new JobItem()
            {
                JobName = jobName,
                JobTimer = jobTimer,
                Job = job,
                JobParam = jobParam,
                Status = SchedulerStatusEnum.Unscheduled
            };
            if (this.JobItems.TryAdd(jobName, item))
            {
                //增加定时器事件处理
                int tally = 0;
                item.JobTimer.Elapsed += (s, e) =>
                {
                    try
                    {
                        item.Job.Execute(new JobContext(item.JobTimer, item.JobParam, ++tally));
                    }
                    catch
                    {

                    }
                };
                return true;
            }
            return false;
        }

        public virtual bool RemoveJob(string jobName)
        {
            if (this.JobItems.TryGetValue(jobName, out JobItem item))
            {
                if (item.Status == SchedulerStatusEnum.Scheduling) return false;
                if (this.JobItems.TryRemove(jobName, out item))
                {
                    this.DisposeSchedule(item);
                }
            }
            return false;
        }

        public virtual bool Exists(string jobName)
        {
            return this.JobItems.ContainsKey(jobName);
        }

        public virtual bool StartJob(string jobName)
        {
            if (this.JobItems.TryGetValue(jobName, out JobItem item))
            {
                if (item.Status == SchedulerStatusEnum.Scheduling) return false;
                //启动调度
                this.BeginSchedule(item);
                return true;
            }
            return false;
        }

        public virtual bool StopJob(string jobName)
        {
            if (this.JobItems.TryGetValue(jobName, out JobItem item))
            {
                if (item.Status == SchedulerStatusEnum.Unscheduled) return false;
                //停止调度
                this.EndSchedule(item);
                return true;
            }
            return false;
        }

        public virtual void Dispose()
        {
            foreach (KeyValuePair<string, JobItem> item in this.JobItems)
            {
                StopJob(item.Key);
                DisposeSchedule(item.Value);
            }
            this.JobItems.Clear();
        }
    }

    public class JobItem
    {
        public string JobName { get; set; }

        public TimerBase JobTimer { get; set; }

        public IJob Job { get; set; }

        public object JobParam { get; set; }

        public SchedulerStatusEnum Status { get; set; }
    }

    public enum SchedulerStatusEnum
    {
        /// <summary>
        /// 未调度
        /// </summary>
        Unscheduled,
        /// <summary>
        /// 调度中
        /// </summary>
        Scheduling
    }
}
