using App.Metrics;
using App.Metrics.Counter;

namespace Application.Metrics
{
    public class MetricsRegistry
    {
        public static CounterOptions CreatedPostsCounter => new App.Metrics.Counter.CounterOptions
        {
            Name = "Created posts",
            Context = "bloggerapi",
            MeasurementUnit = Unit.Calls
        };
    }
}
