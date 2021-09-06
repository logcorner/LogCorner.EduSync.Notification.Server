using LogCorner.EduSync.SignalR.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace LogCorner.EduSync.SignalR.Server.Hubs
{
    //TODO : uncomment when client credential is enabled on Azure AD B2C
    // [Authorize()]
    public class LogCornerHub<T> : Hub<IHubNotifier<T>>, IHubInvoker<T> where T : class
    {
      
        public override Task OnConnectedAsync()
        {
            Console.WriteLine($"OnConnectedAsync :: clientId : {Context.ConnectionId}, clientName : {getClientName()}");
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            Console.WriteLine($"OnDisconnectedAsync :: clientId : {Context.ConnectionId}, clientName : {getClientName()}");
            return base.OnDisconnectedAsync(exception);
        }

        public async Task Publish(T payload)
        {
            await Clients.All.OnPublish(payload);
            Console.WriteLine($"Publish :: payload : {payload} , clientId : {Context.ConnectionId}, clientName : {getClientName()}");
        }

        public async Task PublishToTopic(string topic, T payload)
        {
            await Clients.All.OnPublish(topic, payload);
            Console.WriteLine($"PublishToTopic :: topic : {topic} , payload : {payload}, clientId : {Context.ConnectionId}, clientName : {getClientName()}");
        }

        public async Task Subscribe(string topic)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, topic);
            await Clients.Groups(topic).OnSubscribe(Context.ConnectionId, topic);
            Console.WriteLine($"Subscribe :: topic : {topic} , clientId : {Context.ConnectionId}, clientName : {getClientName()}");
        }

        public async Task UnSubscribe(string topic)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, topic);
            await Clients.Groups(topic).OnUnSubscribe(Context.ConnectionId, topic);
            Console.WriteLine($"UnSubscribe :: topic : {topic} , clientId : {Context.ConnectionId}, clientName : {getClientName()}");
        }

        private string getClientName()
        {
            var httpContext = Context.GetHttpContext();
            var clientName = httpContext.Request.Query["clientName"];
            if (string.IsNullOrWhiteSpace(clientName))
            {
                throw new Exception($"clientName is required ** OnConnectedAsync :: clientId : {Context.ConnectionId}");
            }
            return clientName;
        }
    }
}