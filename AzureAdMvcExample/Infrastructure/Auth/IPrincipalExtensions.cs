using System.Security.Principal;

namespace AzureAdMvcExample.Infrastructure.Auth
{
    public static class IPrincipalExtensions
    {
        public static bool IsInRole(this IPrincipal user, AppRoles role)
        {
            return user.IsInRole(role.ToString());
        }
    }
}