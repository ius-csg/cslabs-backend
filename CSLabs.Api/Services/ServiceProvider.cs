using System;
using System.Reflection;
using AutoMapper;
using CSLabs.Api.Controllers;
using CSLabs.Api.Models;
using CSLabs.Api.Proxmox;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace CSLabs.Api.Services
{
    public static class ServiceProvider
    {
        public static void ConfigureDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContextPool<DefaultContext>(options => options.UseMySql(connectionString, mySqlOptions => {
                // change the version if needed.
                options.UseSnakeCaseNamingConvention();
                mySqlOptions.ServerVersion(new Version(10, 2, 13), ServerType.MariaDb);
            }));
        }

        public static void ProvideAppServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.ProvideProxmoxApi();
            services.AddScoped<BaseControllerDependencies>();
            services.AddTransient<UserLabInstantiationService>();
            services.AddTransient<ProxmoxVmTemplateService>();
            services.AddSingleton<UrlBasedUploadManager>();
        }
    }
}