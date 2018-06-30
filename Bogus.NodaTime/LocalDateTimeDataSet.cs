using System;
using NodaTime;

namespace Bogus.NodaTime
{
    /// <summary>
    /// Methods for generating <see cref="LocalDateTime"/>s.
    /// </summary>
    public class LocalDateTimeDataSet : DataSet
    {
        Func<DateTimeZone> defaultDateTimeZone;

        public LocalDateTimeDataSet(Func<DateTimeZone> defaultDateTimeZone)
        {
            this.defaultDateTimeZone = defaultDateTimeZone;
        }

        /// <summary>
        /// Get a date in the past between <paramref name="reference"/> and years past that date.
        /// </summary>
        /// <param name="daysToGoBack">Days to go back from <paramref name="reference"/>. Default is 100.</param>
        /// <param name="reference">The date to start calculations. Default is SystemClock.Instance.GetCurrentInstant().</param>
        public LocalDateTime Past(int daysToGoBack = 100, LocalDateTime? reference = null)
        {
            var max = ValueOrNull(reference);

            var ticks = TimeSpan.TicksPerDay * daysToGoBack;

            var partTicks = Random.Long(0, ticks);

            var partTimeSpan = Period.FromTicks(partTicks);

            return max - partTimeSpan;
        }

        LocalDateTime ValueOrNull(LocalDateTime? reference)
        {
            if (reference == null)
            {
                return Now();
            }

            return reference.Value;
        }

        LocalDateTime Now()
        {
            var currentInstant = SystemClock.Instance.GetCurrentInstant();
            return currentInstant.InZone(defaultDateTimeZone()).LocalDateTime;
        }

        /// <summary>
        /// Get a date and time that will happen soon.
        /// </summary>
        /// <param name="days">A date no more than N days ahead.</param>
        public LocalDateTime Soon(int days = 10)
        {
            var now = Now();
            return Between(now, now.PlusDays(days));
        }

        /// <summary>
        /// Get a date in the future between <paramref name="reference"/> and years forward of that date.
        /// </summary>
        /// <param name="daysToGoForward">Days to go forward from <paramref name="reference"/>. Default is 100.</param>
        /// <param name="reference">The date to start calculations. Default is SystemClock.Instance.GetCurrentInstant().</param>
        public LocalDateTime Future(int daysToGoForward = 100, LocalDateTime? reference = null)
        {
            var min = ValueOrNull(reference);

            var ticks = TimeSpan.TicksPerDay * daysToGoForward;

            var partTicks = Random.Long(0, ticks);

            var partTimeSpan = Period.FromTicks(partTicks);

            return min + partTimeSpan;
        }

        /// <summary>
        /// Get a random date between start and end.
        /// </summary>
        public LocalDateTime Between(LocalDateTime start, LocalDateTime end)
        {
            var min = LocalDateTime.Min(start, end);
            var max = LocalDateTime.Max(start, end);

            var total = max - min;


            var partTicks = Random.Double() * total.ToDuration().TotalTicks;

            var part = Period.FromTicks(Convert.ToInt64(partTicks));

            return min + part;
        }

        /// <summary>
        /// Get a random date/time within the last few days since now.
        /// </summary>
        /// <param name="days">Number of days to go back.</param>
        public LocalDateTime Recent(int days = 10)
        {
            var now = Now();

            var min = days == 0 ? now : now.Minus(Period.FromDays(days));

            var totalTicks = (now - min).ToDuration().TotalTicks;

            //find % of the timespan
            var partTicks = Random.Double() * totalTicks;

            var part = Period.FromTicks(Convert.ToInt64(partTicks));

            return now - part;
        }
    }
}