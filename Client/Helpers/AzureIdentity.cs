using Azure.Core;
using Azure.Identity;
using CC.Functions.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace CC.Functions.Helpers
{
    public class AzureIdentityAuthProvider : IAuthProvider
    {
        private readonly ILogger _log;
        private readonly AppSettings _settings;
        private dynamic _credential;
        public AzureIdentityAuthProvider(
            ILoggerFactory loggerFactory,
            IOptions<AppSettings> options)
        {
            _log = loggerFactory.CreateLogger<AzureIdentityAuthProvider>();
            _settings = options.Value;
#if DEBUG            
            _credential = new ClientSecretCredential(_settings.TenantId, _settings.ClientAppId, _settings.ClientAppSecret);
#else
            _credential = new ManagedIdentityCredential();
#endif
        }

        public async Task<string> GetAccessToken()
        {

            try
            {
                _log.LogInformation("AzureIdentity - Getting access token");

                var accessTokenRequest = await _credential.GetTokenAsync(
                    new TokenRequestContext(scopes: new string[] { $"{_settings.ServerAppId}/.default" }) { }
                    ) ;

                var accessToken = accessTokenRequest.Token;

                _log.LogInformation((String)accessToken);

                _log.LogInformation("AzureIdentity - Got access token");

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