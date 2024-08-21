using Cronos;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EasyCronJob.Abstractions
{
    /// <summary>
    /// This class includes base operations for Cron Job
    /// </summary>
    public abstract class CronJobService : IHostedService, IDisposable
    {
        private System.Timers.Timer timer;
        private readonly TimeZoneInfo timeZoneInfo;
        private readonly CronFormat cronFormat;
        private readonly CronExpression cronExpression;
        protected CronJobService(string cronExpression, TimeZoneInfo timeZoneInfo, CronFormat cronFormat = CronFormat.Standard)
        {
            this.timeZoneInfo = timeZoneInfo;
            this.cronFormat = cronFormat;
            this.cronExpression = CronExpression.Parse(cronExpression, this.cronFormat);
        }

        protected virtual async Task ScheduleJob(CancellationToken cancellationToken)
        {
            var next = cronExpression.GetNextOccurrence(DateTimeOffset.Now, timeZoneInfo);
            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;
                const double maxDelay = int.MaxValue; // Maximum delay in milliseconds (2147483647 ms)
                if (delay.TotalMilliseconds <= 0)   // prevent non-positive values from being passed into Timer
                {
                    await ScheduleJob(cancellationToken).ConfigureAwait(true);
                    return;
                }

                                
                void StartChunkedTimer(double remainingDelay) // Recursive function to handle timer chunks
                {
                    double currentDelay = Math.Min(remainingDelay, maxDelay);
                
                    timer = new System.Timers.Timer(currentDelay);
                    timer.Elapsed += async (sender, args) =>
                    {
                        timer.Dispose(); // Dispose of the current timer
                        timer = null;
                
                        if (cancellationToken.IsCancellationRequested) return; // Exit if cancellation is requested
                
                        double newRemainingDelay = remainingDelay - currentDelay;
                
                        if (newRemainingDelay > 0) StartChunkedTimer(newRemainingDelay); // Start the next timer chunk
                        else
                        {
                            if (!cancellationToken.IsCancellationRequested)
                            {
                                await DoWork(cancellationToken).ConfigureAwait(true);
                            }
                            if (!cancellationToken.IsCancellationRequested)
                            {
                                await ScheduleJob(cancellationToken).ConfigureAwait(true);
                            }
                        }
                    };
                    timer.Start();
                }
                  
                StartChunkedTimer(delay.TotalMilliseconds); // Start the first timer chunk
            }
            await Task.CompletedTask.ConfigureAwait(true);
        }

        public virtual async Task DoWork(CancellationToken cancellationToken)
        {
            await Task.Delay(50, cancellationToken).ConfigureAwait(true);  // do the work
        }

        public void Dispose()
        {
            timer?.Dispose();
        }

        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            await ScheduleJob(cancellationToken).ConfigureAwait(true);
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Stop();
            await Task.CompletedTask.ConfigureAwait(true);
        }
    }
}
