using EasyCronJob.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EasyCron.Sample.Jobs
{
    public class MyJob : CronJobService
    {
        private readonly ILogger<MyJob> logger;

        public MyJob(ICronConfiguration<MyJob> cronConfiguration, ILogger<MyJob> logger)
            : base(cronConfiguration.CronExpression, cronConfiguration.TimeZoneInfo)
        {
            this.logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Start My Job" + " Start Time : " + DateTime.UtcNow);
            return base.StartAsync(cancellationToken);
        }


        protected override Task ScheduleJob(CancellationToken cancellationToken)
        {
            logger.LogInformation("Schedule My Job" + " Start Time : " + DateTime.UtcNow);
            return base.ScheduleJob(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            logger.LogInformation("Working My Job" + " Start Time : " + DateTime.UtcNow);
            return base.DoWork(cancellationToken);
        }
    }
}
