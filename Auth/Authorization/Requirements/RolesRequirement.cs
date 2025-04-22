using Microsoft.AspNetCore.Authorization;

namespace ApiBase.Auth.Authorization.Requirements
{
    public class RolesRequirement : IAuthorizationRequirement
    {
        public string[] Roles { get; private set; }
        public RolesRequirement(string[] roles)
        {
            Roles = roles;
        }
    }
}
