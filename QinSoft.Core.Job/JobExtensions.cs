using QinSoft.Core.Job.Schedulers;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Job
{
    public static class JobExtensions
    {
        public static bool AddJob(this IScheduler scheduler, JobBuilder jobBuilder)
        {
            return scheduler.AddJob(jobBuilder.JobName, jobBuilder.JobTimer, jobBuilder.Job, jobBuilder.JobParam);
        }

        public static bool AddJob<T>(this IScheduler scheduler, string jobName, double interval, JobFactory jobFactory = null, object jobParam = null) where T : IJob
        {
            return scheduler.AddJob(new JobBuilder()
                .WithName(jobName)
                .WithTimer(interval)
                .WithJobFactory(jobFactory)
                .WithJob<T>()
                .WithParam(jobParam));
        }

        public static bool AddJob<T>(this IScheduler scheduler, string jobName, string cronExpression, JobFactory jobFactory = null, object jobParam = null) where T : IJob
        {
            return scheduler.AddJob(new JobBuilder()
                .WithName(jobName)
                .WithTimer(cronExpression)
                .WithJobFactory(jobFactory)
                .WithJob<T>()
                .WithParam(jobParam));
        }

        public static bool AddJob(this IScheduler scheduler, string jobName, double interval, Type jobType, JobFactory jobFactory = null, object jobParam = null)
        {
            return scheduler.AddJob(new JobBuilder()
                .WithName(jobName)
                .WithTimer(interval)
                .WithJobFactory(jobFactory)
                .WithJob(jobType)
                .WithParam(jobParam));
        }

        public static bool AddJob(this IScheduler scheduler, string jobName, string cronExpression, Type jobType, JobFactory jobFactory = null, object jobParam = null)
        {
            return scheduler.AddJob(new JobBuilder()
                .WithName(jobName)
                .WithTimer(cronExpression)
                .WithJobFactory(jobFactory)
                .WithJob(jobType)
                .WithParam(jobParam));
        }

        public static bool AddJob(this IScheduler scheduler, string jobName, double interval, IJob job, object jobParam = null)
        {
            return scheduler.AddJob(new JobBuilder()
                .WithName(jobName)
                .WithTimer(interval)
                .WithJob(job)
                .WithParam(jobParam));
        }

        public static bool AddJob(this IScheduler scheduler, string jobName, string cronExpression, IJob job, object jobParam = null)
        {
            return scheduler.AddJob(new JobBuilder()
                .WithName(jobName)
                .WithTimer(cronExpression)
                .WithJob(job)
                .WithParam(jobParam));
        }
    }
}
