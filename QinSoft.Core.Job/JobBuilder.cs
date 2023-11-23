using QinSoft.Core.Common.Utils;
using QinSoft.Core.Job.Timers;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Job
{
    /// <summary>
    /// 作业构造
    /// </summary>
    public class JobBuilder
    {
        protected string JobName { get; set; }

        protected double? Interval { get; set; }

        protected string CronExpression { get; set; }

        public IJobFactory JobFactory { get; set; } = new SimpleJobFactory();

        protected Type JobType { get; set; }

        public object JobParam { get; set; }

        public JobBuilder() { }

        public JobBuilder(string jobName)
        {
            ObjectUtils.CheckEmpty(jobName, nameof(jobName));
            this.JobName = jobName;
        }

        public virtual JobBuilder WithName(string jobName)
        {
            ObjectUtils.CheckEmpty(jobName, nameof(jobName));
            this.JobName = jobName;
            return this;
        }

        public virtual JobBuilder WithInterval(double interval)
        {
            ObjectUtils.CheckRange(interval, 0, double.MaxValue, nameof(interval));
            this.Interval = interval;
            return this;
        }

        public virtual JobBuilder WithCron(string cronExpression)
        {
            ObjectUtils.CheckEmpty(cronExpression, nameof(cronExpression));
            this.CronExpression = cronExpression;
            return this;
        }

        public virtual JobBuilder WithJobFactory(IJobFactory jobFactory)
        {
            ObjectUtils.CheckNull(jobFactory, nameof(jobFactory));
            this.JobFactory = jobFactory;
            return this;
        }

        public virtual JobBuilder WithJob<T>() where T : IJob
        {
            return this.WithJob(typeof(T));
        }

        public virtual JobBuilder WithJob(Type type)
        {
            ObjectUtils.CheckNull(type, nameof(type));
            this.JobType = type;
            return this;
        }

        public virtual JobBuilder WithParam(object jobParam)
        {
            this.JobParam = jobParam;
            return this;
        }

        public virtual JobDetail Build()
        {
            TimerBase jobTimer = null;
            if (this.Interval != null)
            {
                jobTimer = new SimpleTimer(this.Interval.Value);
            }
            else if (!string.IsNullOrEmpty(this.CronExpression))
            {
                jobTimer = new CronTimer(this.CronExpression);
            }
            else
            {
                throw new JobException("Lose Interval And CronExpression");
            }
            IJob job = this.JobFactory.CreateJob(this.JobType);

            return new JobDetail()
            {
                JobName = this.JobName,
                JobTimer = jobTimer,
                Job = job,
                JobParam = this.JobParam
            };
        }
    }

    /// <summary>
    /// 作业详情
    /// </summary>
    public class JobDetail
    {
        public string JobName { get; set; }

        public TimerBase JobTimer { get; set; }

        public IJob Job { get; set; }

        public object JobParam { get; set; }
    }
}
