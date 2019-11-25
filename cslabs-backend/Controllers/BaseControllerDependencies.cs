using AutoMapper;
using CSLabsBackend.Config;
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

        public AppSettings AppSettings;
        
        public BaseControllerDependencies(
            DefaultContext defaultContext, 
            IMapper mapper, 
            ProxmoxApi proxmoxApi, 
            IFluentEmailFactory  emailFactory,
            AppSettings appSettings)
        {
            this.DatabaseContext = defaultContext;
            this.Mapper = mapper;
            this.ProxmoxApi = proxmoxApi;
            this.EmailFactory = emailFactory;
            this.AppSettings = appSettings;
        }
    }
}