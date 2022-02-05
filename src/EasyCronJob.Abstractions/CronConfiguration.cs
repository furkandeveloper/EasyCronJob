using Cronos;
using System;

namespace EasyCronJob.Abstractions
{
    /// <summary>
    /// Cron Configuration Object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CronConfiguration<T> : ICronConfiguration<T>
    {
        /// <summary>
        /// Cron Expression. For Example : * * * * *
        /// </summary>
        public string CronExpression { get; set; }

        /// <summary>
        /// Represents any time zone in the world.
        /// </summary>
        public TimeZoneInfo TimeZoneInfo { get; set; }
        
        /// <summary>
        /// Cron Format
        /// Default <see cref="CronFormat.Standard"/>
        /// </summary>
        public CronFormat CronFormat { get; set; } = CronFormat.Standard;
    }
}
