namespace WebAPI.HealthChecks
{
    /// <summary>
    /// Klasa opisuje kondycje usługi
    /// </summary>
    public class HealthCheck
    {
        public string Status { get; set; }
        public string Component { get; set; }
        public string Description { get; set; }
    }
}
