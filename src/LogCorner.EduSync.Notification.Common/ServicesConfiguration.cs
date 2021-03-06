using LogCorner.EduSync.Notification.Common.Authentication;
using LogCorner.EduSync.Notification.Common.Hub;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LogCorner.EduSync.Notification.Common
{
    public static class ServicesConfiguration
    {
        public static void AddSignalRServices(this IServiceCollection services, string endpoint, IConfiguration config)
        {
            services.AddSingleton<ISignalRNotifier, SignalRNotifier>();
            services.AddSingleton<ISignalRPublisher, SignalRPublisher>();
            services.AddHttpContextAccessor();
            services.AddSingleton<IHubInstance, HubConnectionInstance>(ctx =>
            {
                var hubConnectionInstance = new HubConnectionInstance(endpoint, new IdentityProvider(config));

                return hubConnectionInstance;
            });
        }
    }
}