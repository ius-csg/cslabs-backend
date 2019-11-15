using CSLabsBackend.Config;
using Microsoft.Extensions.DependencyInjection;

namespace CSLabsBackend.Proxmox
{
    public static class ProxmoxApiProvider
    {
        public static void ProvideProxmoxApi(this IServiceCollection services, AppSettings appSettings)
        {
            var settings = appSettings.Proxmox;
            services.AddSingleton(new ProxmoxApi(
                settings.Host,
                settings.Username,
                settings.Password
            ));
        }
    }
}