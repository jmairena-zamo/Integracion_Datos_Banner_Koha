using Microsoft.AspNetCore.Authentication;

namespace ApiBase.Auth.Authentication
{
    public class CustomAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "TokenAuthenticationScheme";
    }
}
