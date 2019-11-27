namespace CSLabsBackend.RequestModels
{
    public class EmailVerificationRequest
    {
        public string Type { get; set; }
        public string Code { get; set; }
    }
}