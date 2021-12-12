using System;
using System.Threading.Tasks;
using LogCorner.EduSync.Notification.Common.Model;
using LogCorner.EduSync.Speech.SharedKernel.Events;
using LogCorner.EduSync.Speech.SharedKernel.Serialyser;
using Microsoft.AspNetCore.SignalR.Client;

namespace LogCorner.EduSync.Notification.Common
{
    public class SignalRNotifier : ISignalRNotifier
    {
        public event Action<EventStore> ReceivedOnPublish;

        public event Action<string, object> ReceivedOnPublishToTopic;

        private readonly IHubInstance _hubInstance;
        private readonly IJsonSerializer _eventSerializer;

        public SignalRNotifier(IHubInstance hubInstance, IJsonSerializer eventSerializer)
        {
            _hubInstance = hubInstance;
            _eventSerializer = eventSerializer;
        }

        public async Task StartAsync()
        {
            if (_hubInstance.Connection?.State != HubConnectionState.Connected)
            {
                await _hubInstance.StartAsync();
            }
        }

        public async Task OnPublish()
        {
            _hubInstance.Connection.On<EventStore>(nameof(IHubNotifier<EventStore>.OnPublish),
                u => ReceivedOnPublish?.Invoke(u));
            await Task.CompletedTask;
        }

        public async Task OnPublish(string topic)
        {
            _hubInstance.Connection.On<string, Message>(nameof(IHubNotifier<string>.OnPublish),
                (u, v) =>
                {
                    var payload = _eventSerializer.Deserialize<object>(v.Type, v.Body);
                    ReceivedOnPublishToTopic?.Invoke(u, payload);
                });
            await Task.CompletedTask;
        }

        public async Task StopAsync()
        {
            await _hubInstance.StopAsync();
        }
    }
}