using System.Threading.Tasks;

namespace LogCorner.EduSync.Notification.Common
{
    public interface IIdentityProvider
    {
        Task<string> AcquireTokenForConfidentialClient(string[] scopes);
    }
}