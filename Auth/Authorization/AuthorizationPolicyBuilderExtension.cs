using ApiBase.Auth.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace ApiBase.Auth.Authorization
{
    //Centralización de las políticas
    public static class AuthorizationPolicyBuilderExtension
    {
        public static AuthorizationPolicyBuilder CustomRequiredClaim(this AuthorizationPolicyBuilder builder, string claimType)
        {
            builder.AddRequirements(new ClaimRequirement(claimType));
            return builder;
        }

        public static AuthorizationPolicyBuilder CustomRequiredRoles(this AuthorizationPolicyBuilder builder, params string[] roles)
        {
            builder.AddRequirements(new RolesRequirement(roles));
            return builder;
        }
    }
}
