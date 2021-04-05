using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Extensions.Options;
using CC.Functions.Interfaces;
using System.Net.Http.Headers;
using System.Collections.Generic;

namespace CC.Functions
{
    public class ProcessOrdersTimer
    {
        private readonly ILogger _log;
        private readonly AppSettings _settings;
        private readonly HttpClient _httpClient;
        private readonly IAuthProvider _authProvider;
        private readonly IEnumerable<IAuthProvider> _authProviders;

        public ProcessOrdersTimer(
            ILoggerFactory loggerFactory,
            IOptions<AppSettings> options,
            HttpClient httpClient,
            IAuthProvider authProvider,
            IEnumerable<IAuthProvider> authProviders
        )
        {
            _log = loggerFactory.CreateLogger<ProcessOrdersTimer>();
            _settings = options.Value;
            _httpClient = httpClient;
            _authProvider = authProvider;
            _authProviders = authProviders;
        }

        [FunctionName("ProcessOrdersTimer")]
        public async Task Run([TimerTrigger("0 */10 * * * *")]TimerInfo myTimer, ILogger log)
        {
            var orders = new List<object>();
            
            _log.LogInformation("ProcessOrders function processed a request.");

            foreach (var authProvider in _authProviders)
            {
                var accessToken = await authProvider.GetAccessToken();

                if (accessToken != null)
                {
                    _httpClient.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", accessToken);

                    HttpResponseMessage getOrdersResult =
                        await _httpClient.GetAsync(_settings.ServerAppGetOrdersUrl);

                    if (getOrdersResult != null && getOrdersResult.IsSuccessStatusCode)
                    {
                        _log.LogInformation("Got orders from server");

                        Stream contentStream = await getOrdersResult.Content.ReadAsStreamAsync();

                        using (var streamReader = new StreamReader(contentStream))
                        using (var jsonReader = new JsonTextReader(streamReader))
                        {
                            JsonSerializer serializer = new JsonSerializer();
                            orders.Add(serializer.Deserialize<object>(jsonReader));

                            //* Process the data
                        }
                    }
                }
            }

            //* If only one Auth Provider is registered then

            /*

            var orders = new object();

            var accessToken = await _authProvider.GetAccessToken();

            if(accessToken != null)
            {
                _httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue("Bearer", accessToken);
                
                HttpResponseMessage getOrdersResult = 
                    await _httpClient.GetAsync(_settings.ServerAppGetOrdersUrl);

                if(getOrdersResult != null && getOrdersResult.IsSuccessStatusCode)
                {
                    _log.LogInformation("Got orders from server");

                    Stream contentStream = await getOrdersResult.Content.ReadAsStreamAsync();

                    using (var streamReader = new StreamReader(contentStream))
                    using (var jsonReader = new JsonTextReader(streamReader))
                    {
                        JsonSerializer serializer = new JsonSerializer();
                        orders = serializer.Deserialize<object>(jsonReader);

                        //* Process the data
                    }
                } 
            }

            */
        }
    }
}
