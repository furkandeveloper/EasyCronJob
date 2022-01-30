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
    }
}
