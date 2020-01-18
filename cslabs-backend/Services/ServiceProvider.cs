using System;
using CSLabsBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace CSLabsBackend.Services
{
    public static class ServiceProvider
    {
        public static void ConfigureDatabase(this IServiceCollection services, string connectionString)
        {
            services.AddDbContextPool<DefaultContext>(options => options.UseMySql(connectionString, mySqlOptions => {
                // change the version if needed.
                mySqlOptions.ServerVersion(new Version(10, 2, 13), ServerType.MariaDb);
            }));
        }
    }
}