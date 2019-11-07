using CSLabsBackend.Config;
using Microsoft.Extensions.DependencyInjection;

namespace CSLabsBackend.Proxmox
{
    public static class ProxmoxApiProvider
    {
        public static void ProvideProxmoxApi(this IServiceCollection services, AppSettings appSettings)
        {
            var rundeckSettings = appSettings.RunDeckApi;
            services.AddSingleton(new ProxmoxApi(
                rundeckSettings.Scheme,
                rundeckSettings.Host,
                rundeckSettings.ApiKey,
                rundeckSettings.JobIdIds
            ));
        }
    }
}