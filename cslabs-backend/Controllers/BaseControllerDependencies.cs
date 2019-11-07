using AutoMapper;
using CSLabsBackend.Models;
using CSLabsBackend.Models.UserModels;
using CSLabsBackend.Proxmox;

namespace CSLabsBackend.Controllers
{
    public class BaseControllerDependencies
    {
        public  DefaultContext DatabaseContext { get; }
        public IMapper Mapper { get; }
        public ProxmoxApi ProxmoxApi { get;}
        
        public BaseControllerDependencies(DefaultContext defaultContext, IMapper mapper, ProxmoxApi proxmoxApi)
        {
            this.DatabaseContext = defaultContext;
            this.Mapper = mapper;
            this.ProxmoxApi = proxmoxApi;
        }
    }
}