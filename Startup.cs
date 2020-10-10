using Microsoft.Extensions.DependencyInjection;
using SSCMS.Login.Abstractions;
using SSCMS.Login.Core;
using SSCMS.Plugins;

namespace SSCMS.Login
{
    public class Startup : IPluginConfigureServices
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IOAuthRepository, OAuthRepository>();
            services.AddScoped<ILoginManager, LoginManager>();
        }
    }
}
