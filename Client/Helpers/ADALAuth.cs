using CC.Functions.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Threading.Tasks;

namespace CC.Functions.Helpers
{
    public class ADALAuthProvider : IAuthProvider
    {
        private readonly ILogger _log;
        private readonly AppSettings _settings;
        private readonly ClientCredential _clientCredential;
        private readonly AuthenticationContext _authContext;
        
        public ADALAuthProvider(
            ILoggerFactory loggerFactory,
            IOptions<AppSettings> options)
        {
            _log = loggerFactory.CreateLogger<ADALAuthProvider>();
            _settings = options.Value;
            _clientCredential = new ClientCredential(_settings.ClientAppId, _settings.ClientAppSecret);
            _authContext = new AuthenticationContext($"https://login.microsoftonline.com/{_settings.TenantId}");
        }

        public async Task<string> GetAccessToken()
        {

            try
            {
                _log.LogInformation("ADAL - Getting access token");

                //ADAL

                if (_settings.ClearTokenCache)
                    _authContext.TokenCache.Clear();

                var token = await _authContext.AcquireTokenAsync(_settings.ServerAppId, _clientCredential);
                var accessToken = token.AccessToken;

                _log.LogInformation(accessToken);

                _log.LogInformation("ADAL - Got access token");

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