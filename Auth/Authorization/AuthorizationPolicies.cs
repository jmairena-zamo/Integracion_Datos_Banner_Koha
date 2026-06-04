using Microsoft.AspNetCore.Authorization;
using Integracion_Datos_Banner_Koha.Auth.Authorization.Requirements;

namespace Integracion_Datos_Banner_Koha.Auth.Authorization
{
    public static class AuthorizationPolicies
    {
        public static AuthorizationPolicy RequiredClaim(string claimType)
        {
            return new AuthorizationPolicyBuilder("TOKEN", "APIKEY")
            .RequireAuthenticatedUser()
            .AddRequirements(new ClaimRequirement(claimType))
            .Build();
        }

        public static AuthorizationPolicy RequiredRoles(params string[] roles)
        {
            return new AuthorizationPolicyBuilder("TOKEN", "APIKEY")
            .RequireAuthenticatedUser()
            .AddRequirements(new RolesRequirement(roles))
            .Build();
        }
    }
}
