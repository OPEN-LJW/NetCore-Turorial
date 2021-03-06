﻿using CustomHost.Internal;
using CustomHost.Internal.Implementation;
using CustomHost.Startup;
using CustomHost.Startup.Implementation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace CustomHost
{
    public static class ServiceHostBuilderExtensions
    {
        public static IServiceHostBuilder UseStartup(this IServiceHostBuilder hostBuilder, Type startupType)
        {
            return hostBuilder
                .ConfigureServices(services =>
                {
                    if (typeof(IStartup).GetTypeInfo().IsAssignableFrom(startupType.GetTypeInfo()))
                    {
                        services.AddSingleton(typeof(IStartup), startupType);
                    }
                    else
                    {
                        services.AddSingleton(typeof(IStartup), sp =>
                        {
                            return new ConventionBasedStartup(StartupLoader.LoadMethods(sp, startupType, ""));
                        });

                    }
                });
        }

        public static IServiceHostBuilder UseStartup<TStartup>(this IServiceHostBuilder hostBuilder) where TStartup : class
        {
            return hostBuilder.UseStartup(typeof(TStartup));
        }
    }
}
