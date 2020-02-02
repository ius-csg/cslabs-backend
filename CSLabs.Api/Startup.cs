using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CSLabs.Api.Config;
using CSLabs.Api.Controllers;
using CSLabs.Api.Models;
using CSLabs.Api.Services;
using CSLabs.Api.Proxmox;
using CSLabs.Api.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using Sgw.KebabCaseRouteTokens;

namespace CSLabs.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        readonly string CorsPolicyName = "_CorsPolicy";

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettings = ConfigureAppSettings(services);
            services.AddRouting(options =>
            {
                options.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
                options.LowercaseUrls = true;
            });
            services
                .AddMvc(options =>
                {
                    options.Conventions.Add(new RouteTokenTransformerConvention(
                        new SlugifyParameterTransformer()));
                    options.EnableEndpointRouting = false;
                    // options.Conventions.Add(new KebabCaseRouteTokenReplacementControllerModelConvention());
                    // options.Conventions.Add(
                    //     new KebabCaseRouteTokenReplacementActionModelConvention("Create", "Delete", "Update", "Get", "Find"));
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            ConfigureEmail(services, appSettings.Email);
            services.ConfigureDatabase(appSettings.ConnectionStrings.DefaultConnection);
            ConfigureCors(services, appSettings.CorsUrls);
            ConfigureJWT(services, appSettings.JWTSecret);
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.ProvideAppServices();
        }

        private void ConfigureEmail(IServiceCollection services, EmailSettings emailSettings)
        {
            Console.WriteLine(Environment.CurrentDirectory);
            services
                .AddFluentEmail(emailSettings.FromAddress)
                .AddRazorRenderer(Path.Join(Environment.CurrentDirectory, "Views"))
                .AddSmtpSender(emailSettings.Host, emailSettings.Port, emailSettings.UserName, emailSettings.Password);
        }
        
        private AppSettings ConfigureAppSettings(IServiceCollection services)
        {
            var appSettings = Configuration.Get<AppSettings>();
            services.AddSingleton(appSettings);
            return appSettings;
        }
        private void ConfigureCors(IServiceCollection services, string[] corsUrls)
        {
            services.AddCors(options => { 
                options.AddPolicy(CorsPolicyName, builder => {
                    builder.WithOrigins(corsUrls)
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    }); 
            });
        }


        private void ConfigureJWT(IServiceCollection services, string secret)
        {
            // configure jwt authentication
            
            var key = Encoding.ASCII.GetBytes(secret);
            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseCors(CorsPolicyName);
            app.UseRouting();
//            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller:slugify}/{action:slugify}/{id?}");
            });
        }
    }
}
