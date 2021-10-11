using System.Threading.Tasks;

namespace LogCorner.EduSync.SignalR.Common
{
    public interface IIdentityProvider
    {
        Task<string> AcquireTokenForConfidentialClient(string[] scopes);
    }
}