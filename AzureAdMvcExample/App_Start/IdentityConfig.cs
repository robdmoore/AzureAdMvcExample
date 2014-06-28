using System;
using System.Configuration;
using System.IdentityModel.Claims;
using System.IdentityModel.Tokens;
using System.Web.Helpers;

namespace AzureAdMvcExample
{
    public static class IdentityConfig
    {
        public static void ConfigureIdentity()
        {
            AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.Name;
            RefreshIssuerKeys();
        }

        private static void RefreshIssuerKeys()
        {
            // http://msdn.microsoft.com/en-us/library/azure/dn641920.aspx
            var configPath = AppDomain.CurrentDomain.BaseDirectory + "\\" + "Web.config";
            var metadataAddress = ConfigurationManager.AppSettings["ida:FederationMetadataLocation"];
            ValidatingIssuerNameRegistry.WriteToConfig(metadataAddress, configPath);
        }
    }
}