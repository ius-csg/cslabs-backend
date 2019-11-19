using AutoMapper;
using CSLabsBackend.Models;
using CSLabsBackend.Models.UserModels;
using CSLabsBackend.Proxmox;
using FluentEmail.Core;

namespace CSLabsBackend.Controllers
{
    public class BaseControllerDependencies
    {
        public  DefaultContext DatabaseContext { get; }
        public IMapper Mapper { get; }
        public ProxmoxApi ProxmoxApi { get;}

        public IFluentEmailFactory EmailFactory { get; set; }
        
        public BaseControllerDependencies(DefaultContext defaultContext, IMapper mapper, ProxmoxApi proxmoxApi, IFluentEmailFactory  emailFactory)
        {
            this.DatabaseContext = defaultContext;
            this.Mapper = mapper;
            this.ProxmoxApi = proxmoxApi;
            this.EmailFactory = emailFactory;
        }
    }
}