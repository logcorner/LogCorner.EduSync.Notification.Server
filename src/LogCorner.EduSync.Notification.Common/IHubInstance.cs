using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;

namespace LogCorner.EduSync.Notification.Common
{
    public interface IHubInstance
    {
        HubConnection Connection { get; }

        Task StartAsync();

        Task StopAsync();
    }
}