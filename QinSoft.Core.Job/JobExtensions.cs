using QinSoft.Core.Job.Schedulers;
using QinSoft.Core.Job.Timers;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Job
{
    public static class JobExtensions
    {
        public static bool AddJob(this IScheduler scheduler, JobDetail jobBuilder)
        {
            return scheduler.AddJob(jobBuilder.JobName, jobBuilder.JobTimer, jobBuilder.Job, jobBuilder.JobParam);
        }

        public static bool AddJob<T>(this IScheduler scheduler, string jobName, double interval, SimpleJobFactory jobFactory = null, object jobParam = null) where T : IJob
        {
            if (jobFactory == null) jobFactory = new SimpleJobFactory();
            return scheduler.AddJob(new JobBuilder()
                .WithName(jobName)
                .WithInterval(interval)
                .WithJobFactory(jobFactory)
                .WithJob<T>()
                .WithParam(jobParam)
                .Build());
        }

        public static bool AddJob<T>(this IScheduler scheduler, string jobName, string cronExpression, SimpleJobFactory jobFactory = null, object jobParam = null) where T : IJob
        {
            if (jobFactory == null) jobFactory = new SimpleJobFactory();
            return scheduler.AddJob(new JobBuilder()
                .WithName(jobName)
                .WithCron(cronExpression)
                .WithJobFactory(jobFactory)
                .WithJob<T>()
                .WithParam(jobParam)
                .Build());
        }

        public static bool AddJob(this IScheduler scheduler, string jobName, double interval, Type jobType, SimpleJobFactory jobFactory = null, object jobParam = null)
        {
            if (jobFactory == null) jobFactory = new SimpleJobFactory();
            return scheduler.AddJob(new JobBuilder()
                .WithName(jobName)
                .WithInterval(interval)
                .WithJobFactory(jobFactory)
                .WithJob(jobType)
                .WithParam(jobParam)
                .Build());
        }

        public static bool AddJob(this IScheduler scheduler, string jobName, string cronExpression, Type jobType, SimpleJobFactory jobFactory = null, object jobParam = null)
        {
            if (jobFactory == null) jobFactory = new SimpleJobFactory();
            return scheduler.AddJob(new JobBuilder()
                .WithName(jobName)
                .WithCron(cronExpression)
                .WithJobFactory(jobFactory)
                .WithJob(jobType)
                .WithParam(jobParam)
                .Build());
        }
    }
}
