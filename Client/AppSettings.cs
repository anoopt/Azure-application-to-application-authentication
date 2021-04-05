
namespace CC.Functions {
    public class AppSettings {
        public string ClientAppId { get; set; }
        public string ClientAppSecret { get; set; }
        public string ServerAppId { get; set; }
        public string ServerAppGetOrdersUrl { get; set; }
        public string TenantId { get; set; }
        public bool ClearTokenCache { get; set; }
    }
}

//* Sample local.settings.json

/*

{
  "IsEncrypted": false,
  "Values": {
    "FUNCTIONS_WORKER_RUNTIME": "dotnet",
    "AppSettings:ServerAppId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
    "AppSettings:ClientAppId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
    "AppSettings:ClientAppSecret": "**********************************",
    "AppSettings:TenantId": "xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
    "AppSettings:ClearTokenCache": "false",
    "AppSettings:ServerAppGetOrdersUrl": "https://yourappname.azurewebsites.net/api/GetOrders?code=xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx",
    "AzureWebJobsStorage": "UseDevelopmentStorage=true"
  }
}

*/