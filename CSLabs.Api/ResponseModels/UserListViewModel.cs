using System.Collections.Generic;
using CSLabs.Api.Models.UserModels;

namespace CSLabs.Api.ResponseModels
{
    public class UserListViewModel
    {
        
        public List<UserViewModel> UserList { get; set; }
        
        public UserListViewModel (List<User> usersList)
        {
            UserList = new List<UserViewModel>();
            
            foreach (var user in usersList)
            {
                UserViewModel userView = new UserViewModel();
                userView.Id = user.Id;
                userView.Email = user.Email;
                userView.Role = user.Role;
                userView.FirstName = user.FirstName;
                userView.MiddleName = user.MiddleName;
                userView.LastName = user.LastName;
                
                UserList.Add(userView);
            }
            
        }
        
    }

    public class UserViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public EUserRole Role { get; set; } = EUserRole.Guest;
    }
}