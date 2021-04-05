using CC.Functions.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Threading.Tasks;
using ClientCredential = Microsoft.IdentityModel.Clients.ActiveDirectory.ClientCredential;

namespace CC.Functions.Helpers
{
    public class MSALAuthProvider : IAuthProvider
    {
        private readonly ILogger _log;
        private readonly AppSettings _settings;
        private readonly string[] _scopes;
        private readonly IConfidentialClientApplication _msalClient;

        public MSALAuthProvider(
            ILoggerFactory loggerFactory,
            IOptions<AppSettings> options)
        {
            _log = loggerFactory.CreateLogger<MSALAuthProvider>();
            _settings = options.Value;
            _scopes = new string[] { _settings.ServerAppId + "/.default" };
            _msalClient = ConfidentialClientApplicationBuilder
                        .Create(_settings.ClientAppId)
                        .WithClientSecret(_settings.ClientAppSecret)
                        //.WithCertificate(cert)
                        .WithAuthority(AadAuthorityAudience.AzureAdMyOrg, true)
                        .WithTenantId(_settings.TenantId)
                        .Build();
        }

        public async Task<string> GetAccessToken()
        {

            try
            {
                _log.LogInformation("MSAL - Getting access token");

                
                // NOTE: This will return a cached token if a valid one exists
                // Set ClearTokenCache to true if a new one is needed all the time

                var authResult = await _msalClient
                            .AcquireTokenForClient(_scopes)
                            .WithForceRefresh(_settings.ClearTokenCache)
                            .ExecuteAsync();

                var accessToken = authResult.AccessToken;

                _log.LogInformation(accessToken);

                _log.LogInformation("MSAL - Got access token");

                return accessToken;
            }
            catch (Exception ex)
            {
                _log.LogError("Auth - error getting access token");
                _log.LogError(ex.Message);
                return null;
            }
        }
    }
}