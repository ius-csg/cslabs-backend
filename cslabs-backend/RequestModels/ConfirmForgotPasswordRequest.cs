namespace CSLabsBackend.RequestModels
{
    public class ConfirmForgotPasswordRequest
    {
        public string PasswordRecoveryCode { get; set; }
        public string NewPassword { get; set; }
    }
}