using EasyCronJob.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EasyCron.Sample.Jobs
{
    public class ConsoleCronJob : CronJobService
    {
        private readonly ILogger<ConsoleCronJob> logger;

        public ConsoleCronJob(ICronConfiguration<ConsoleCronJob> cronConfiguration, ILogger<ConsoleCronJob> logger) 
            : base(cronConfiguration.CronExpression,cronConfiguration.TimeZoneInfo)
        {
            this.logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Start");
            return base.StartAsync(cancellationToken);
        }


        protected override Task ScheduleJob(CancellationToken cancellationToken)
        {
            logger.LogInformation("Scheduled");
            return base.ScheduleJob(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            logger.LogInformation("Do Work");
            return base.DoWork(cancellationToken);
        }
    }
}
