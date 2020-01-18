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
        public ProxmoxManager ProxmoxManager { get;}

        public IFluentEmailFactory EmailFactory { get; set; }

        public AppSettings AppSettings;
        
        public BaseControllerDependencies(
            DefaultContext defaultContext, 
            IMapper mapper, 
            ProxmoxManager proxmoxManager, 
            IFluentEmailFactory  emailFactory,
            AppSettings appSettings)
        {
            this.DatabaseContext = defaultContext;
            this.Mapper = mapper;
            this.ProxmoxManager = proxmoxManager;
            this.EmailFactory = emailFactory;
            this.AppSettings = appSettings;
        }
    }
}