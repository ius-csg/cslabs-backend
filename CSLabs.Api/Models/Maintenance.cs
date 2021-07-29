namespace CSLabs.Api.Models
{
    public class Maintenance
    {
        public int Id { get; set; }
        public bool IsMaintenanceMode { get; set; }
        public bool IsRestorationTimeKnown { get; set; }
        public int RestorationTime { get; set; }
    }
}