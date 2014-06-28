using System.IdentityModel.Claims;
using System.Web.Helpers;

namespace AzureAdMvcExample
{
    public static class IdentityConfig
    {
        public static void ConfigureIdentity()
        {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;
        }
    }
}