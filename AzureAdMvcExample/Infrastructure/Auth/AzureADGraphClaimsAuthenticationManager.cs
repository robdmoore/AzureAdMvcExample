using System.Configuration;
using System.Security.Claims;

namespace AzureAdMvcExample.Infrastructure.Auth
{
    public class AzureADGraphClaimsAuthenticationManager : ClaimsAuthenticationManager
    {
        public override ClaimsPrincipal Authenticate(string resourceName, ClaimsPrincipal incomingPrincipal)
        {
            if (incomingPrincipal == null || !incomingPrincipal.Identity.IsAuthenticated)
                return incomingPrincipal;

            // Ideally this should be the code below so the connection is resolved from a DI container, but for simplicity of the demo I'll leave it as a new statement
            //var graphConnection = DependencyResolver.Current.GetService<IAzureADGraphConnection>();
            var graphConnection = new AzureADGraphConnection(
                ConfigurationManager.AppSettings["AzureADTenant"],
                ConfigurationManager.AppSettings["ida:ClientId"],
                ConfigurationManager.AppSettings["ida:Password"]);

            var roles = graphConnection.GetRolesForUser(incomingPrincipal);
            foreach (var r in roles)
                ((ClaimsIdentity)incomingPrincipal.Identity).AddClaim(
                    new Claim(ClaimTypes.Role, r.ToString(), ClaimValueTypes.String, "GRAPH"));
            return incomingPrincipal;
        }
    }
}
