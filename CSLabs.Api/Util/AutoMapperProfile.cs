using AutoMapper;
using CSLabs.Api.Models.UserModels;
using CSLabs.Api.RequestModels;
using CSLabs.Api.Models;

namespace CSLabs.Api.Util
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegistrationRequest, User>();
        }
    }
}