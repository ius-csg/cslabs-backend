using System;
using FluentEmail.Core;
using Microsoft.Extensions.DependencyInjection;

namespace CSLabs.Api.Email
{
    public class AppFluentEmailFactory : IFluentEmailFactory 
    {
        private IServiceProvider services;

        public AppFluentEmailFactory(IServiceProvider services) => this.services = services;

        public IFluentEmail Create() => services.GetService<IFluentEmail>().ReplyTo("iuscompsec@gmail.com");
    }
}