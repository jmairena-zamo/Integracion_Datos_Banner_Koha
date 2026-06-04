using Microsoft.AspNetCore.Authorization;

namespace Integracion_Datos_Banner_Koha.Auth.Authorization.Requirements
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
