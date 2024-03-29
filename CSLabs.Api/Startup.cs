﻿using System;
using System.IO;
using System.Reflection;
using System.Text;
using Amazon;
using AutoMapper;
using CSLabs.Api.Config;
using CSLabs.Api.Email;
using CSLabs.Api.Jobs;
using CSLabs.Api.Services;
using CSLabs.Api.Util;
using FluentEmail.Core;
using FluentScheduler;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
                    // Allow enums to be converted to strings and vice versa in requests
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                });

            ConfigureEmail(services, appSettings);
            services.ConfigureDatabase(appSettings.ConnectionStrings.DefaultConnection);
            ConfigureCors(services, appSettings.CorsUrls);
            ConfigureJWT(services, appSettings.JWTSecret);
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.ProvideAppServices();
        }

        private void ConfigureEmail(IServiceCollection services, AppSettings appSettings)
        {
            if (string.IsNullOrEmpty(appSettings.Email.FromAddress))
                throw new ConfigurationException("Email.FromAddress must be configured in the appsettings.json. Please follow the setup steps in the readme.");
            var serviceBuilder = services.AddTransient<IFluentEmailFactory, AppFluentEmailFactory>();
            var useAwsSes = !string.IsNullOrEmpty(appSettings.Email.AwsSes.SecretKey) ||
                            !string.IsNullOrEmpty(appSettings.Email.AwsSes.AccessKey);
            var baseConfig = serviceBuilder
                .AddFluentEmail(appSettings.Email.FromAddress)
                .AddRazorRenderer(Path.Join(Environment.CurrentDirectory, "Views"));
            if (appSettings.Email.DisableEmail)
                baseConfig.AddMockSender();
            else if(useAwsSes)
                baseConfig.AddSESSender(appSettings.Email.AwsSes.AccessKey, appSettings.Email.AwsSes.SecretKey, RegionEndpoint.USEast2);
            else 
                baseConfig.AddSmtpSender(appSettings.Email.Host, appSettings.Email.Port, appSettings.Email.UserName, appSettings.Email.Password);
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
            
            // Initialize Fluent Scheduler. Jobs are scheduled in the JobRegistry class
            JobManager.Initialize(new JobRegistry(app.ApplicationServices));
            
            
        }
    }
}
