namespace CSLabs.Api.Config
{
    public class EmailSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FromAddress { get; set; }

        public bool DisableEmail { get; set; } = false;
        public AWSSesSettings AwsSes { get; set; } = new AWSSesSettings();
    }
}