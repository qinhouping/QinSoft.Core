using System;
using System.Collections.Generic;
using System.Text;

namespace QinSoft.Core.Job
{
    public class JobFactory
    {
        public JobFactory()
        {
        }

        public virtual IJob CreateJob(Type type)
        {
            if (!typeof(IJob).IsAssignableFrom(type)) return null;
            return Activator.CreateInstance(type) as IJob;
        }

        public virtual IJob CreateJob<T>() where T : IJob
        {
            return Activator.CreateInstance<T>();
        }
    }
}
