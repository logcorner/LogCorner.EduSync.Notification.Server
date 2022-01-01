using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

namespace LogCorner.EduSync.Notification.Common
{
    public interface IHubInstance
    {
        HubConnection Connection { get; }

        Task StartAsync();

        Task StopAsync();
    }
}