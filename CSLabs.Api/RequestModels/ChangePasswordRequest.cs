namespace CSLabs.Api.RequestModels
{
    public class ChangePasswordRequest
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }

        public bool ValidatePasswordStrength()
        {
            return Zxcvbn.Core.EvaluatePassword(NewPassword).Score >= 4;
        }
    }
}
