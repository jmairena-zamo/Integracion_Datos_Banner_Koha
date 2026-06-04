using Microsoft.AspNetCore.Authentication;

namespace Integracion_Datos_Banner_Koha.Auth.Authentication
{
    public class CustomAuthenticationOptions : AuthenticationSchemeOptions
    {
        public const string DefaultScheme = "TokenAuthenticationScheme";
    }
}
