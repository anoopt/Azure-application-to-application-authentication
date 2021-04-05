using CC.Functions.Helpers;
using CC.Functions.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(CC.Functions.Startup))]

namespace CC.Functions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddHttpClient();
            
            builder.Services.AddOptions<AppSettings>()
            .Configure<IConfiguration>((settings, configuration) =>
            {
                configuration.GetSection("AppSettings").Bind(settings);
            });

            builder.Services.AddSingleton<IAuthProvider, ADALAuthProvider>();
            builder.Services.AddSingleton<IAuthProvider, MSALAuthProvider>();
            builder.Services.AddSingleton<IAuthProvider, AppAuthLibAuthProvider>();
            builder.Services.AddSingleton<IAuthProvider, AzureIdentityAuthProvider>();
        }
    }
}