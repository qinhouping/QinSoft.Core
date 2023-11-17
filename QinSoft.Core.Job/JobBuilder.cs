using QinSoft.Core.Job.Timers;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Job
{
    public class JobBuilder
    {
        public string JobName { get; protected set; }

        public TimerBase JobTimer { get; protected set; }

        public JobFactory JobFactory { get; protected set; } = new JobFactory();

        public IJob Job { get; protected set; }

        public object JobParam { get; set; }

        public JobBuilder() { }

        public JobBuilder(string jobName)
        {
            this.JobName = jobName;
        }

        public virtual JobBuilder WithName(string jobName)
        {
            this.JobName = jobName;
            return this;
        }

        public virtual JobBuilder WithTimer(double interval)
        {
            this.JobTimer = new SimpleTimer(interval);
            return this;
        }

        public virtual JobBuilder WithTimer(string cronExpression)
        {
            this.JobTimer = new CronTimer(cronExpression);
            return this;
        }

        public virtual JobBuilder WithJob<T>(T job) where T : IJob
        {
            this.Job = job;
            return this;
        }

        public virtual JobBuilder WithJobFactory(JobFactory jobFactory)
        {
            if (jobFactory != null)
                this.JobFactory = jobFactory;
            return this;
        }

        public virtual JobBuilder WithJob<T>() where T : IJob
        {
            this.Job = this.JobFactory.CreateJob<T>();
            return this;
        }

        public virtual JobBuilder WithJob(Type type)
        {
            this.Job = this.JobFactory.CreateJob(type);
            return this;
        }

        public virtual JobBuilder WithParam(object jobParam)
        {
            this.JobParam = jobParam;
            return this;
        }
    }
}
