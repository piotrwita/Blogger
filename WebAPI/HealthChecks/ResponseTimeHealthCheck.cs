using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace WebAPI.HealthChecks
{
    /// <summary>
    /// Sprawdza szybkość sieci
    /// </summary>
    public class ResponseTimeHealthCheck : IHealthCheck
    {
        private Random random = new Random();
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            int ResponseTimeMS = random.Next(1, 300);

            if (ResponseTimeMS < 100)
            {
                return Task.FromResult(HealthCheckResult.Healthy($"The response time looks good ({ResponseTimeMS})"));
            }
            else if (ResponseTimeMS < 200)
            {
                return Task.FromResult(HealthCheckResult.Degraded($"The response time is a bit slow ({ResponseTimeMS})"));
            }
            else
            {
                return Task.FromResult(HealthCheckResult.Unhealthy($"The response time is unacceptable ({ResponseTimeMS})"));
            }
        }
    }
}
