using EasyCronJob.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCronJob.Core
{
    /// <summary>
    /// Apply Resulation
    /// </summary>
    /// <typeparam name="T">
    /// Cron Job Service in CronJobService type
    /// </typeparam>
    /// <param name="services">
    /// Microsoft.Extensions.DependencyInjection.IServiceCollection
    /// </param>
    /// <param name="action">
    /// Cron Configuration Action Object
    /// </param>
    /// <returns>
    /// Microsoft.Extensions.DependencyInjection.IServiceCollection
    /// </returns>
    public static class Startup
    {
        public static IServiceCollection ApplyResulation<T>(this IServiceCollection services, Action<ICronConfiguration<T>> action)
            where T : CronJobService
        {
            var options = new CronConfiguration<T>();
            action.Invoke(options);
            services.AddSingleton<ICronConfiguration<T>>(options);
            services.AddHostedService<T>();
            return services;
        }
    }
}
