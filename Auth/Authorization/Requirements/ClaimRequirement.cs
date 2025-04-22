using Microsoft.AspNetCore.Authorization;

namespace ApiBase.Auth.Authorization.Requirements
{
    public class ClaimRequirement: IAuthorizationRequirement
    {
        public string ClaimType { get; }
        public ClaimRequirement(string claimType)
        {
            ClaimType = claimType;
        }
    }
}
