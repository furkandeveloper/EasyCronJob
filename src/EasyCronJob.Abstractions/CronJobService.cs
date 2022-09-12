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
        private bool _active = true;

        private System.Timers.Timer timer;
        private readonly TimeZoneInfo timeZoneInfo;
        private readonly CronFormat cronFormat;
        private readonly CronExpression cronExpression;
        protected CronJobService(string cronExpression, TimeZoneInfo timeZoneInfo, CronFormat cronFormat = CronFormat.Standard)
        {         
            this.timeZoneInfo = timeZoneInfo;
            this.cronFormat = cronFormat;
            this.cronExpression = CronExpression.Parse(cronExpression, this.cronFormat);

            this.ActiveAttributeCheck();
        }

        private void ActiveAttributeCheck()
        {
            this.GetType().CheckAttribute<JobActiveAttribute>().Do(() =>
            {
                this._active = this.GetType().GetAttributeValue((JobActiveAttribute d) => d.Active);
            });

            this.GetType().CheckAttribute<JobPassiveAttribute>().Do(() =>
            {
                this._active = false;
            });
        }
        public void SetActive(bool active)
        {
            this._active = active;
        }

        protected virtual async Task ScheduleJob(CancellationToken cancellationToken)
        {
            if (!_active) return;

            var next = cronExpression.GetNextOccurrence(DateTimeOffset.Now, timeZoneInfo);
            if (next.HasValue)
            {
                var delay = next.Value - DateTimeOffset.Now;
                if (delay.TotalMilliseconds <= 0)   // prevent non-positive values from being passed into Timer
                {
                    await ScheduleJob(cancellationToken).ConfigureAwait(true);
                }
                timer = new System.Timers.Timer(delay.TotalMilliseconds);
                timer.Elapsed += async (sender, args) =>
                {
                    timer.Dispose();  // reset and dispose timer
                    timer = null;

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await DoWork(cancellationToken).ConfigureAwait(true);
                    }

                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await ScheduleJob(cancellationToken).ConfigureAwait(true);    // reschedule next
                    }
                };
                timer.Start();
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
