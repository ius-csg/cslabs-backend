using System.Threading.Tasks;
using CSLabs.Api.RequestModels;
using NUnit.Framework;

namespace CSLabs.Tests
{
    public class UserControllerTest
    {
        [Test]
        public void TestPasswordEnforcement()
        {
            var registrationRequest = new RegistrationRequest
            {
                FirstName = "John",
                LastName = "Test",
                ConfirmPassword = "password",
                Email = "test@test.com",
                MiddleName = "Test",
                Password = "password"
            };
            
            Assert.False(registrationRequest.ValidatePasswordStrength());

            registrationRequest.Password = "abc123";
            Assert.False(registrationRequest.ValidatePasswordStrength());
            
            registrationRequest.Password = "trustno1";
            Assert.False(registrationRequest.ValidatePasswordStrength());
            
            registrationRequest.Password = "ncc1701";
            Assert.False(registrationRequest.ValidatePasswordStrength());
            
            registrationRequest.Password = "iloveou!";
            Assert.False(registrationRequest.ValidatePasswordStrength());
            
            registrationRequest.Password = "primetime21";
            Assert.False(registrationRequest.ValidatePasswordStrength());
            
            registrationRequest.Password = "*&OSr8U6TR#uJLxB$V5@7EFMKnbYeV*F";
            Assert.True(registrationRequest.ValidatePasswordStrength());
        }
    }
}