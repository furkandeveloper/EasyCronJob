using Cronos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyCronJob.Abstractions
{
    /// <summary>
    /// This interface includes base parameter for Cron Job.
    /// </summary>
    /// <typeparam name="T">
    /// Cron Job
    /// </typeparam>
    public interface ICronConfiguration<T>
    {
        /// <summary>
        /// Cron Expression. For Example; '****' Cron.Minutely
        /// </summary>
        public string CronExpression { get; set; }

        /// <summary>
        /// TimeZone Information
        /// </summary>
        public TimeZoneInfo TimeZoneInfo { get; set; }

        /// <summary>
        /// Cron Format
        /// Default <see cref="CronFormat.Standard"/>
        /// </summary>
        public CronFormat CronFormat { get; set; }
    }
}
