using Cronos;
using EasyCronJob.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCronJob.AutoConfigurer
{
    /// <summary>
    /// This class includes Auto Configure for Cron Services base methods
    /// </summary>
    public static class Startup
    {
        /// <summary>
        /// Find Cron Job Services
        /// </summary>
        /// <returns>
        /// Returns <see cref="List{Type}"/>
        /// </returns>
        private static List<Type> FindCronJobServices()
        {
            var baseCronJobType = typeof(CronJobService);
            var cronJobServices = AppDomain.CurrentDomain.GetAssemblies().SelectMany(s => s.GetTypes())
                                .Where(p => baseCronJobType.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract)
                                .ToList();
            return cronJobServices;
        }

        /// <summary>
        /// This method takes <see cref="IServiceCollection"/> service collection and <see cref="string"/> service name.
        /// </summary>
        /// <param name="services">
        /// <see cref="IServiceCollection"/>
        /// </param>
        /// <param name="serviceName">
        /// <see cref="string"/>
        /// </param>
        /// <returns>
        /// Returns <see cref="Tuple{T1, T2}"/>
        /// </returns>
        private static Tuple<string, TimeZoneInfo, CronFormat> FindServiceParameter(IServiceCollection services, string serviceName)
        {
            var serviceProvider = services.BuildServiceProvider();
            var configurationObject = serviceProvider.GetRequiredService<IConfiguration>();
            string cronExpression = configurationObject.GetValue<string>("CronJobs:" + serviceName + ":CronExpression");
            string info = configurationObject.GetValue<string>("CronJobs:" + serviceName + ":TimeZoneInfo");
            string cronFormat = configurationObject.GetValue<string>("CronJobs:" + serviceName + ":CronFormat");
            TimeZoneInfo timeZoneInfo;
            switch (info)
            {
                case "Local":
                    timeZoneInfo = TimeZoneInfo.Local;
                    break;
                case "Utc":
                    timeZoneInfo = TimeZoneInfo.Utc;
                    break;
                default:
                    timeZoneInfo = TimeZoneInfo.Local;
                    break;
            }
            CronFormat format = CronFormat.Standard;
            switch (cronFormat)
            {
                case "Standard":
                    format = CronFormat.Standard;
                    break;
                case "IncludeSeconds":
                    format = CronFormat.IncludeSeconds;
                    break;
                default:
                    format = CronFormat.Standard;
                    break;
            }

            return new Tuple<string, TimeZoneInfo, CronFormat>(cronExpression, timeZoneInfo, format);
        }

        /// <summary>
        /// This method performs Initialize Cron Services. Returns <see cref="IServiceCollection"/>
        /// </summary>
        /// <param name="services">
        /// <see cref="IServiceCollection"/>
        /// </param>
        /// <returns>
        /// <see cref="IServiceCollection"/>
        /// </returns>
        public static IServiceCollection InitializeCronServices(this IServiceCollection services)
        {
            var cronJobServices = FindCronJobServices();
            foreach (var item in cronJobServices)
            {
                var cronConfigurationInterfaceType = typeof(ICronConfiguration<>);
                var cronConfigurationType = typeof(CronConfiguration<>);
                var genericInterfaceType = cronConfigurationInterfaceType.MakeGenericType(item);
                var genericType = cronConfigurationType.MakeGenericType(item);
                services.AddSingleton(genericInterfaceType, genericType);
            }
            return services;
        }

        /// <summary>
        /// This method performs Auto Configurer for Cron Services.
        /// </summary>
        /// <param name="services">
        /// <see cref="IServiceCollection"/>
        /// </param>
        /// <returns>
        /// <see cref="IServiceCollection"/>
        /// </returns>
        public static IServiceCollection AutoConfigurer(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var cronJobServices = FindCronJobServices();
            foreach (var item in cronJobServices)
            {
                ConfigureCronServices(services, item, serviceProvider);
            }
            return services;
        }

        /// <summary>
        /// This method takes <see cref="IServiceCollection"/> service collection, <see cref="Type"/> type of cron service and <see cref="IServiceProvider"/> service provider. This method perform configure cron services and managemenet. Returns <see cref="IServiceCollection"/> service collection
        /// </summary>
        /// <param name="services">
        /// <see cref="IServiceCollection"/> service Collection
        /// </param>
        /// <param name="item">
        /// <see cref="Type"/> type of cron services
        /// </param>
        /// <param name="serviceProvider">
        /// <see cref="IServiceProvider"/> service provider
        /// </param>
        /// <returns>
        /// <see cref="IServiceCollection"/> service collection
        /// </returns>
        private static IServiceCollection ConfigureCronServices(IServiceCollection services, Type item, IServiceProvider serviceProvider)
        {
            var cronParameters = FindServiceParameter(services, item.Name);
            if (string.IsNullOrWhiteSpace(cronParameters.Item1))
                return services;
            var ctors = item.GetConstructors().FirstOrDefault();
            var parameters = ctors.GetParameters();
            List<object> ctorServices = new List<object>();
            foreach (var parameter in parameters)
            {
                var findedService = serviceProvider.GetService(parameter.ParameterType);
                if (findedService.GetType().Name.Contains("CronConfiguration"))
                {
                    findedService.GetType().GetProperty("CronExpression").SetValue(findedService, cronParameters.Item1);
                    findedService.GetType().GetProperty("TimeZoneInfo").SetValue(findedService, cronParameters.Item2);
                    findedService.GetType().GetProperty("CronFormat").SetValue(findedService, cronParameters.Item3);
                }
                ctorServices.Add(findedService);
            }

            var cronService = (CronJobService)Activator.CreateInstance(item, args: ctorServices.ToArray());
            services.TryAddEnumerable(ServiceDescriptor.Singleton(typeof(IHostedService),cronService));
            return services;
        }
    }
}
