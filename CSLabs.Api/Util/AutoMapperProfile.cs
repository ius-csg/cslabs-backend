using AutoMapper;
using CSLabs.Api.Models.UserModels;
using CSLabs.Api.RequestModels;
using CSLabs.Api.Models;
using CSLabs.Api.Models.ModuleModels;

namespace CSLabs.Api.Util
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegistrationRequest, User>();
            CreateMap<UserLab, UserLab>();
            CreateMap<LabRequest, Lab>();
        }
    }
}