<p align="center">
  <img src="https://user-images.githubusercontent.com/47147484/121789342-dcf22600-cbdd-11eb-8394-c7dca1a95f97.png" style="max-width:100%;" height="140" />
</p>

[![CodeFactor](https://www.codefactor.io/repository/github/furkandeveloper/easycronjob/badge)](https://www.codefactor.io/repository/github/furkandeveloper/easycronjob)
<a href="https://gitmoji.carloscuesta.me">
  <img src="https://img.shields.io/badge/gitmoji-%20😜%20😍-FFDD67.svg?style=flat-square" alt="Gitmoji">
</a>
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

![Nuget](https://img.shields.io/nuget/dt/EasyCronJob.Core?label=EasyCronJob.Core%20Downloads)
![Nuget](https://img.shields.io/nuget/v/EasyCronJob.Core?label=EasyCronJob.Core)
![Nuget](https://img.shields.io/nuget/dt/EasyCronJob.Abstractions?label=EasyCronJob.Abstractions%20Downloads)
![Nuget](https://img.shields.io/nuget/v/EasyCronJob.Abstractions?label=EasyCronJob.Abstractions)

***

## What is Cron Job?
**Cron** is a program used to repeat a task on a system. A task assignment is a cron job if it does not command a repeat as a whole.

### How does a cron job work?

If you want to schedule a task once at a later time, you can use another command like it. But for repetitive tasks, cron is a great solution.

  
## Cron Services

You can write your own CronJob classes using the CronJobService abstract class.
We've included a sample cron job for you.
When this cron job runs, it logs the related job on the console.

First, install the EasyCronJob.Core library on your application via [Nuget](https://www.nuget.org/packages/EasyCronJob.Core/).

You can now create your own cron jobs.

```csharp
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
```
  
  ## Startup Configuration
  
For the cron job you created, you must configure it in Startup.cs.

Do not worry. This configuration is pretty simple.

```csharp
services.ApplyResulation<ConsoleCronJob>(options =>
{
     options.CronExpression = "* * * * *";
     options.TimeZoneInfo = TimeZoneInfo.Local;
});
```

Process completed. Now, your cron job will run when the cron expression value you specified comes.

  ## Crontab.guru

Cron values ​​can be confusing at times.
You don't have to memorize them.
You can use the [crontab.guru](https://crontab.guru/) website to generate the cron values ​​you want or to learn the runtime of an existing cron value.

![image](https://user-images.githubusercontent.com/47147484/121820030-25224e80-cc99-11eb-82c0-059688736ed0.png)
  
  ***
  
### Documentation
Visit [Wiki](https://github.com/furkandeveloper/EasyCronJob/wiki) page for documentation.

***

![image](https://user-images.githubusercontent.com/47147484/121820542-17ba9380-cc9c-11eb-9961-f8a882aa7607.png)
