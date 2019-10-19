using AutoMapper;
using CSLabsBackend.Models;
using CSLabsBackend.RequestModels;

namespace CSLabsBackend.Util
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegistrationRequest, User>();
        }
    }
}