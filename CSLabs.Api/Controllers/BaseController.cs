using System;
using System.Linq;
using AutoMapper;
using CSLabs.Api.Config;
using CSLabs.Api.Models;
using CSLabs.Api.Models.UserModels;
using CSLabs.Api.Proxmox;
using FluentEmail.Core;
using Microsoft.AspNetCore.Mvc;

namespace CSLabs.Api.Controllers
{
    public class BaseController: ControllerBase
    {
        protected readonly DefaultContext DatabaseContext;
        protected IMapper Mapper { get;}
        private User user = null;
        protected readonly ProxmoxManager ProxmoxManager;
        private IFluentEmailFactory EmailFactory { get; }
        
        protected string WebAppUrl { get; }

        protected AppSettings AppSettings { get;}
        
        protected IFluentEmail CreateEmail()
        {
            return EmailFactory.Create();
        }

        protected T Map<T>(object value)
        {
            return Mapper.Map<T>(value);
        }
        
        public BaseController(BaseControllerDependencies dependencies)
        {
            this.DatabaseContext = dependencies.DatabaseContext;
            this.Mapper = dependencies.Mapper;
            this.ProxmoxManager = dependencies.ProxmoxManager;
            this.EmailFactory = dependencies.EmailFactory;
            this.WebAppUrl = dependencies.AppSettings.WebAppUrl;
            this.AppSettings = dependencies.AppSettings;
        }

        public User GetUser()
        {
            if (user != null)
                return user;
            if (this.User.Identity.Name == null)
                return null;
            try
            {
                user = this.DatabaseContext.Users.SingleOrDefault(u => u.Id == int.Parse(this.User.Identity.Name));
                return user;
            }
            catch (ArgumentNullException) { return null; }
            catch (FormatException) { return null; }
            catch (OverflowException) { return null; }
        }
    }
}