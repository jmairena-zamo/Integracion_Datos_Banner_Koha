using Microsoft.AspNetCore.Authorization;

namespace Integracion_Datos_Banner_Koha.Auth.Authorization.Requirements
{
    public class ClaimRequirement : IAuthorizationRequirement
    {
        public string ClaimType { get; }
        public ClaimRequirement(string claimType)
        {
            ClaimType = claimType;
        }
    }
}
