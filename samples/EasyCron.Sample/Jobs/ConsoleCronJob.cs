using EasyCronJob.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EasyCron.Sample.Jobs
{
    public class ConsoleCronJob : CronJobService
    {
        private readonly ILogger<ConsoleCronJob> logger;

        public ConsoleCronJob(ICronConfiguration<ConsoleCronJob> cronConfiguration, ILogger<ConsoleCronJob> logger) 
            : base(cronConfiguration.CronExpression,cronConfiguration.TimeZoneInfo, cronConfiguration.CronFormat)
        {
            this.logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Start Console Job" + " Start Time : " + DateTime.UtcNow);
            return base.StartAsync(cancellationToken);
        }


        protected override Task ScheduleJob(CancellationToken cancellationToken)
        {
            logger.LogInformation("Schedule Console Job" + " Start Time : " + DateTime.UtcNow);
            return base.ScheduleJob(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            logger.LogInformation("Working Console Job" + " Start Time : " + DateTime.UtcNow);
            return base.DoWork(cancellationToken);
        }
    }
}
