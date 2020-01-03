using System;
using AspectCore.Extensions.DependencyInjection;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Yunyong.EventBus.EasyNetQ
{
    public static class EventBusRegisterExtensions
    {
        public static IServiceCollection RegisterEasyNetQ(this IServiceCollection services,
            EventBusConfig config)
        {
            config.PrefetchCount = Math.Min(10, Math.Max(1, config.PrefetchCount));
            var connString =
                $"host={config.HostName.Trim('"')};virtualHost={config.VirtualHost.Trim('"')};username={config.UserName.Trim('"')};password={config.Password.Trim('"')};prefetchcount={config.PrefetchCount};timeout=20";
            services.AddSingleton(
                RabbitHutch.CreateBus(connString));

            services.AddSingleton<IEventBus, EventBusEasyNetQ>(p =>
                new EventBusEasyNetQ(p.GetService<IBus>(), p, config.Prefix, p.GetService<ILoggerFactory>()));

            services.AddTransient<ServiceActionInvokeTrackAttribute>();
            return services;
        }

        public static IServiceProvider RegisterEasyNetQProvider(this IServiceCollection services,
            EventBusConfig configuration)
        {
            //注入权限监控（需要调用EventBus查询权限）
            //services.AddScoped<IUserPermissionService, EventService>();

            services.RegisterEasyNetQ(configuration);

            //return services.BuildAspectInjectorProvider();

            return services.BuildDynamicProxyProvider();
        }
    }
}