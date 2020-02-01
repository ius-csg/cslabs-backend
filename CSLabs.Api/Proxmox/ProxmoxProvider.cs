using CSLabs.Api.Config;
using Microsoft.Extensions.DependencyInjection;

namespace CSLabs.Api.Proxmox
{
    public static class ProxmoxProvider
    {
        public static void ProvideProxmoxApi(this IServiceCollection services)
        {
            services.AddScoped<ProxmoxManager>();
        }
    }
}