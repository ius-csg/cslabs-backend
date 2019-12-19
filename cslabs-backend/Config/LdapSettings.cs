namespace CSLabsBackend.Config
{
    public class LdapSettings
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string AdminDn { get; set; }
        public string AdminPassword { get; set; }
    }
}