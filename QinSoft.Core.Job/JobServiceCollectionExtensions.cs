using Microsoft.Extensions.DependencyInjection.Extensions;
using QinSoft.Core.Common.Utils;
using QinSoft.Core.Job.Schedulers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class JobServiceCollectionExtensions
    {
        /// <summary>
        /// 注入Database
        /// </summary>
        public static IServiceCollection AddJobScheduler(this IServiceCollection services)
        {
            ObjectUtils.CheckNull(services, "services");
            services.AddOptions();
            services.TryAdd(ServiceDescriptor.Singleton<IScheduler, SimpleScheduler>());
            return services;
        }
    }
}
