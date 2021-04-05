using CC.Functions.Interfaces;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace CC.Functions.Helpers
{
    public class AppAuthLibAuthProvider : IAuthProvider
    {
        private readonly ILogger _log;
        private readonly AppSettings _settings;
        private readonly AzureServiceTokenProvider _provider;

        public AppAuthLibAuthProvider(
            ILoggerFactory loggerFactory,
            IOptions<AppSettings> options)
        {
            _log = loggerFactory.CreateLogger<AppAuthLibAuthProvider>();
            _settings = options.Value;
#if DEBUG
            _provider = new AzureServiceTokenProvider($"RunAs=App;AppId={_settings.ClientAppId};TenantId={_settings.TenantId};AppKey={_settings.ClientAppSecret}");
#else
            _provider = new AzureServiceTokenProvider("RunAs=App");
#endif
        }

        public async Task<string> GetAccessToken()
        {

            try
            {
                _log.LogInformation("AppAuthLib - Getting access token");

                var accessToken = await _provider.GetAccessTokenAsync(_settings.ServerAppId, _settings.ClearTokenCache);

                _log.LogInformation(accessToken);

                _log.LogInformation("AppAuthLib - Got access token");

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