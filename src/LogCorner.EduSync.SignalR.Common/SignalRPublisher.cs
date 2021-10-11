﻿using LogCorner.EduSync.SignalR.Common.Model;
using LogCorner.EduSync.Speech.SharedKernel.Serialyser;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace LogCorner.EduSync.SignalR.Common
{
    public class SignalRPublisher : ISignalRPublisher
    {
        private readonly IHubInstance _hubInstance;
        private readonly IJsonSerializer _eventSerializer;

        public SignalRPublisher(IHubInstance hubInstance, IJsonSerializer eventSerializer)
        {
            _hubInstance = hubInstance;
            _eventSerializer = eventSerializer;
        }

        public async Task SubscribeAsync(string topic)
        {
            if (_hubInstance.Connection?.State != HubConnectionState.Connected)
            {
                await _hubInstance.StartAsync();
            }
            await _hubInstance.Connection.InvokeAsync(nameof(IHubInvoker<string>.Subscribe), topic);
        }

        public async Task PublishAsync<T>(string topic, T payload)
        {
            try
            {
                if (_hubInstance.Connection?.State != HubConnectionState.Connected)
                {
                    await _hubInstance.StartAsync();
                }

                var serializedBody = _eventSerializer.Serialize(payload);

                var type = payload.GetType().AssemblyQualifiedName;
                var message = new Message(type, serializedBody);

                await _hubInstance.Connection.InvokeAsync(nameof(IHubInvoker<Message>.PublishToTopic), topic, message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}