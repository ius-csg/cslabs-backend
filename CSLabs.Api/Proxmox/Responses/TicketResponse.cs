namespace CSLabs.Api.Proxmox.Responses
{
    public class TicketResponse
    {
        public int Port { get; set; }
        public string Ticket { get; set; }
        public string Cert { get; set; }
        public string Upid { get; set; }
        public string User { get; set; }
    }
}