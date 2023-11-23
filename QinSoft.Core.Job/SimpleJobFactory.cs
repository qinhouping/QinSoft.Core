using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Job
{
    public interface IJobFactory
    {
        IJob CreateJob(Type type);
    }

    public class SimpleJobFactory : IJobFactory
    {
        public SimpleJobFactory()
        {
        }

        public virtual IJob CreateJob(Type type)
        {
            if (!typeof(IJob).IsAssignableFrom(type)) return null;
            return Activator.CreateInstance(type) as IJob;
        }
    }

    public class JobFactory : SimpleJobFactory, IJobFactory
    {
        protected IServiceProvider serviceProvider;
        public JobFactory(IServiceProvider provider)
        {
            this.serviceProvider = provider;
        }

        public override IJob CreateJob(Type type)
        {
            if (!typeof(IJob).IsAssignableFrom(type)) return null;
            return serviceProvider.GetService(type) as IJob;
        }
    }
}
