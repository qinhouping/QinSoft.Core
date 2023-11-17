using Microsoft.VisualStudio.TestTools.UnitTesting;
using QinSoft.Core.Job.Schedulers;
using QinSoft.Core.Job.Timers;
using QinSoft.Core.Job;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using QinSoft.Core.Common.Utils;
using Microsoft.Extensions.DependencyInjection;

namespace QinSoft.Core.Test
{
    [TestClass]
    public class JobUnitTest
    {
        [TestMethod]
        public void TestSimpleTimer()
        {
            TimerBase timer = new SimpleTimer(1000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

            Thread.Sleep(1000 * 3600);
        }

        [TestMethod]
        public void TestCronTimer()
        {
            TimerBase timer = new CronTimer("1-2 * * * * ?");
            timer.Elapsed += Timer_Elapsed;
            timer.Start();

            Thread.Sleep(1000 * 3600);
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Debug.WriteLine(sender.GetType().FullName + " timer elapsed");
        }

        [TestMethod]
        public void TestJobScheduler()
        {
            IScheduler scheduler = new SimpleScheduler();
            scheduler.AddJob<TestJob>("test_job", "1-2 * * * * ?", new IocJobFactory(), "测试参数");
            scheduler.StartJob("test_job");

            Thread.Sleep(1000 * 3600);
        }
    }

    class TestJob : IJob
    {
        public void Execute(JobContext context)
        {
            Debug.WriteLine("execute job:" + context.ToJson());
        }
    }

    class IocJobFactory : JobFactory
    {
        public override IJob CreateJob(Type type)
        {
            return Programe.ServiceProvider.GetService(type) as IJob;
        }

        public override IJob CreateJob<T>()
        {
            return Programe.ServiceProvider.GetService<T>();
        }
    }
}
