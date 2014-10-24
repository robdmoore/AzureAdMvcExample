using System.Web.Mvc;

namespace AzureAdMvcExample.Infrastructure.Auth
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params AppRoles[] allowedRoles)
        {
            Roles = string.Join(",", allowedRoles);
        }
    }
}