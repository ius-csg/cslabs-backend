namespace CSLabs.Api.Config
{
    public class NoVncSettings
    {
        public string FastBaseUrl { get; set; }
        public string ReliableBaseUrl { get; set; }
        public bool UseHttpsForHealthCheckRequest { get; set; }
        public string HealthCheckUrl { get; set; }
    }
}