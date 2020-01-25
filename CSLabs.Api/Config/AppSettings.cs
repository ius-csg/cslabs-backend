using System.Collections.Generic;

namespace CSLabs.Api.Config
{
    public class AppSettings
    {
        public string JWTSecret { get; set; }
        public string ModuleSpecialCode { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public EmailSettings Email { get; set; }
        
        public LdapSettings Ldap { get; set; }
        public string[] CorsUrls { get; set; }
        
        public string ProxmoxEncryptionKey { get; set; }
        
        public string WebAppUrl { get; set; }
    }
}