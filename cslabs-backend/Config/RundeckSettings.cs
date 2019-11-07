namespace CSLabsBackend.Config
{
    public class RundeckSettings
    {
        public string Scheme { get; set; }
        public string Host { get; set; }
        public string ApiKey { get; set; }
        public RundeckJobIdSettings JobIds { get; set; }
        
    }
}