using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using System;
using System.Threading.Tasks;

namespace LogCorner.EduSync.SignalR.Common
{
    public class IdentityProvider : IIdentityProvider
    {
        private readonly IConfiguration _configuration;

        public IdentityProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> AcquireTokenForConfidentialClient(string[] scopes)
        {
            string tenantId = _configuration["AzureAdConfideantialClient:TenantId"];
            string authority = $"https://login.microsoftonline.com/{tenantId}";

            string clientSecret = _configuration["AzureAdConfideantialClient:ClientSecret"];

            string clientId = _configuration["AzureAdConfideantialClient:ClientId"];

            var app =
                       ConfidentialClientApplicationBuilder.Create(clientId)
                           .WithClientSecret(clientSecret)
                           .WithAuthority(new Uri(authority))
                           .Build();

            AuthenticationResult result;
            try
            {
                result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Token acquired \n");
                Console.ResetColor();
            }
            catch (MsalServiceException ex) when (ex.Message.Contains("AADSTS70011"))
            {
                // Invalid scope. The scope has to be of the form "https://resourceurl/.default"
                // Mitigation: change the scope to be as expected
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Scope provided is not supported");
                Console.ResetColor();
                throw;
            }

            return result?.AccessToken;
        }
    }
}