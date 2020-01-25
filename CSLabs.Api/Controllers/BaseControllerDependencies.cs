using AutoMapper;
using CSLabs.Api.Config;
using CSLabs.Api.Models;
using CSLabs.Api.Proxmox;
using CSLabs.Api.Models.UserModels;
using FluentEmail.Core;

namespace CSLabs.Api.Controllers
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