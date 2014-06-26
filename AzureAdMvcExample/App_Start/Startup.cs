using System.Configuration;
using Microsoft.Owin.Security.ActiveDirectory;
using Owin;

namespace AzureAdMvcExample
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseWindowsAzureActiveDirectoryBearerAuthentication(
                new WindowsAzureActiveDirectoryBearerAuthenticationOptions
                {
                    Audience = ConfigurationManager.AppSettings["ida:AudienceUri"],
                    Tenant = ConfigurationManager.AppSettings["AzureADTenant"]
                });
        }
    }
}