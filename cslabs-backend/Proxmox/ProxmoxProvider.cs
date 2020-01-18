using CSLabsBackend.Config;
using Microsoft.Extensions.DependencyInjection;

namespace CSLabsBackend.Proxmox
{
    public static class ProxmoxProvider
    {
        public static void ProvideProxmoxApi(this IServiceCollection services)
        {
            services.AddScoped<ProxmoxManager>();
        }
    }
}