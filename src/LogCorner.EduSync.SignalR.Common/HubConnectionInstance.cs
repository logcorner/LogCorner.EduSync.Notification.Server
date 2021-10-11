using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;

namespace LogCorner.EduSync.SignalR.Common
{
    public class HubConnectionInstance : IHubInstance
    {
        private readonly IIdentityProvider _identityProvider;

        private readonly string _url;
        public HubConnection Connection { get; private set; }

        public HubConnectionInstance(string url, IIdentityProvider identityProvider)
        {
            _url = url;
            _identityProvider = identityProvider;
        }

        public async Task StartAsync()
        {
            try
            {
                var accessToken = await InitConfidentialClientAsync();

                Connection = new HubConnectionBuilder()
                    .WithUrl(_url, options =>
                    {
                        options.AccessTokenProvider = () => Task.FromResult(accessToken);
                    })
                    .Build();

                await Connection.StartAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task StopAsync()
        {
            if (Connection != null && Connection.State != HubConnectionState.Disconnected)
            {
                await Connection.StopAsync();
            }
        }

        private async Task<string> InitConfidentialClientAsync()
        {
            var scopes = new[] { "https://datasynchrob2c.onmicrosoft.com/signalr/hub/.default" };

            var accessToken = await _identityProvider.AcquireTokenForConfidentialClient(scopes);
            return accessToken;
        }
    }
}