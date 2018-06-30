using NodaTime;

namespace Bogus.NodaTime
{
    public class NodaTimeDataSet : DataSet
    {
        public InstantDataSet Instant { get; }

        public NodaTimeDataSet()
        {
            Instant = new InstantDataSet
            {
                Random = Random
            };
        }

        /// <summary>
        /// Get a random <see cref="Duration"/>. Default 1 week/7 days.
        /// </summary>
        public Duration Duration(Duration? maximum = null)
        {
            var span = maximum ?? global::NodaTime.Duration.FromDays(7);

            var partTimeSpanTicks = Random.Double() * span.TotalTicks;

            return global::NodaTime.Duration.FromTicks(partTimeSpanTicks);
        }
    }
}