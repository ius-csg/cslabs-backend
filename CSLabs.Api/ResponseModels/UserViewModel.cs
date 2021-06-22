using CSLabs.Api.Models.UserModels;

namespace CSLabs.Api.ResponseModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public EUserRole Role { get; set; }
        
        public UserViewModel(User user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            MiddleName = user.MiddleName;
            LastName = user.LastName;
            Email = user.Email;
            Role = user.Role;
        }
    }
}