namespace CSLabs.Api.Proxmox.Responses
{
    public class ApiTokenResponse
    {
        public string Value { get; set; }
        public TokenInfo Info { get; set; }
        public string FullTokenId { get; set; }
    }

    public class TokenInfo
    {
        public string Comment { get; set; }
        public int Expire { get; set; }
        public bool PrivSep { get; set; }
    }
}