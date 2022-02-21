using System.Collections.Generic;

namespace CSLabs.Api.Config
{
    public class AWSSettings
    {
        public SESConfig SES { get; set; }
        public string AccessKey { get; set; }
        public string SecretKey { get; set; }
    }

    public class SESConfig
    {
        public string FromAddress;
        public string ReplyToAddress;
    }
}