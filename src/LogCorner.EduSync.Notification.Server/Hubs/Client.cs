using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace LogCorner.EduSync.Notification.Server.Hubs
{
    public class Client
    {
        public readonly string ConnectedUser;
        public readonly string ClientName;

        public Client(HttpContext httpContext, string clientName)
        {
            ConnectedUser = httpContext.User.Claims.FirstOrDefault(c => c.Type == "name"
                                                                      || c.Type == ClaimTypes.NameIdentifier)?.Value;

            ClientName = clientName;
        }
    }
}