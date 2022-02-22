using System.Collections.Generic;

namespace CSLabs.Api.Config
{
    public class AWSSettings
    {
        public SESSettings SES { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
    }

    public class SESSettings
    {
        public string FromAddress { get; set; }
        public string DevFromAddress { get; set; }
    }
}