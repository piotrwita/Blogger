namespace WebAPI.HealthChecks
{
    /// <summary>
    /// Klasa opisuje ogolna kondycje api i dostarcza informacje o kondycji poszczegolnych uslug
    /// </summary>
    public class HealthCheckResponse
    {
        public string Status { get; set; }
        public IEnumerable<HealthCheck> Checks { get; set; }
        public TimeSpan Duration { get; set; }  
    }
}
