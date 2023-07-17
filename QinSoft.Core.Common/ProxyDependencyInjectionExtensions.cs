using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection.Extensions
{
    /// <summary>
    /// 代理依赖注入扩展，
    /// 支持Castle拦截器
    /// </summary>
    public static class ProxyDependencyInjectionExtensions
    {
        /// <summary>
        /// Castle代理对象工厂
        /// </summary>
        public static Func<IServiceProvider, object> ProxyObjectFactory(Type serviceType, Type implementationType, params Type[] interceptorTypes)
        {
            return (IServiceProvider provider) =>
            {
                var target = provider.GetService(implementationType);
                List<IInterceptor> interceptors = interceptorTypes.Select(interceptorType => provider.GetService(interceptorType) as IInterceptor).ToList();
                return new ProxyGenerator().CreateInterfaceProxyWithTarget(serviceType, target, interceptors.ToArray());
            };
        }

        /// <summary>
        ///  增加代理依赖注入
        /// </summary>
        public static IServiceCollection TryProxyAdd(this IServiceCollection services, Type serviceType, Type implementationType, ServiceLifetime serviceLifetime, params Type[] interceptorTypes)
        {
            services.TryAdd(new ServiceDescriptor(implementationType, implementationType, serviceLifetime));
            services.TryAdd(new ServiceDescriptor(serviceType, ProxyObjectFactory(serviceType, implementationType, interceptorTypes), serviceLifetime));

            return services;
        }

        /// <summary>
        ///  增加代理依赖注入，ServiceLifetime:Scoped
        /// </summary>
        public static IServiceCollection TryProxyAddScoped(this IServiceCollection services, Type serviceType, Type implementationType, params Type[] interceptorTypes)
        {
            return TryProxyAdd(services, serviceType, implementationType, ServiceLifetime.Scoped, interceptorTypes);
        }

        /// <summary>
        ///  增加代理依赖注入，ServiceLifetime:Scoped
        /// </summary>
        public static IServiceCollection TryProxyAddScoped<TService, TImplementation>(this IServiceCollection services, params Type[] interceptorTypes)
            where TService : class
            where TImplementation : class, TService
        {
            return TryProxyAdd(services, typeof(TService), typeof(TImplementation), ServiceLifetime.Scoped, interceptorTypes);
        }

        /// <summary>
        ///  增加代理依赖注入，ServiceLifetime:Singleton
        /// </summary>
        public static IServiceCollection TryProxyAddSingleton(this IServiceCollection services, Type serviceType, Type implementationType, params Type[] interceptorTypes)
        {
            return TryProxyAdd(services, serviceType, implementationType, ServiceLifetime.Singleton, interceptorTypes);
        }

        /// <summary>
        ///  增加代理依赖注入，ServiceLifetime:Singleton
        /// </summary>
        public static IServiceCollection TryProxyAddSingleton<TService, TImplementation>(this IServiceCollection services, params Type[] interceptorTypes)
            where TService : class
            where TImplementation : class, TService
        {
            return TryProxyAdd(services, typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton, interceptorTypes);
        }

        /// <summary>
        ///  增加代理依赖注入，ServiceLifetime:Transient
        /// </summary>
        public static IServiceCollection TryProxyAddTransient(this IServiceCollection services, Type serviceType, Type implementationType, params Type[] interceptorTypes)
        {
            return TryProxyAdd(services, serviceType, implementationType, ServiceLifetime.Transient, interceptorTypes);
        }

        /// <summary>
        ///  增加代理依赖注入，ServiceLifetime:Transient
        /// </summary>
        public static IServiceCollection TryProxyAddTransient<TService, TImplementation>(this IServiceCollection services, params Type[] interceptorTypes)
            where TService : class
            where TImplementation : class, TService
        {
            return TryProxyAdd(services, typeof(TService), typeof(TImplementation), ServiceLifetime.Transient, interceptorTypes);
        }
    }
}
