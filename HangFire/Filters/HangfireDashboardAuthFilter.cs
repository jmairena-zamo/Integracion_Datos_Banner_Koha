using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace Integracion_Datos_Banner_Koha.Hangfire.Filters
{
    public class HangfireDashboardAuthFilter : IDashboardAuthorizationFilter
    {
        private readonly IAuthorizationService _authorizationService;

        public HangfireDashboardAuthFilter(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            // 1. FORZAR LA EJECUCIÓN DEL HANDLER
            // Esto obliga a que tu TokenAuthenticationHandler.HandleAuthenticateAsync() se ejecute AHORA.
            var authResult = httpContext.AuthenticateAsync("TOKEN_APIKEY").Result;

            if (!authResult.Succeeded)
            {
                authResult = httpContext.AuthenticateAsync("CookieAuth").GetAwaiter().GetResult();
            }

            if (!authResult.Succeeded)
            {
                // IMPORTANTE: Redireccionamos manualmente por código para evitar el 401 del Handler
                var loginUrl = "/auth/login-hangfire?returnUrl=/hangfire";
                httpContext.Response.Clear();
                httpContext.Response.StatusCode = 302; // Redirect
                httpContext.Response.Headers["Location"] = loginUrl;
                return false;
            }

            if (httpContext.User.Identity?.IsAuthenticated == false || httpContext.Request.Query.ContainsKey("api_key"))
            {
                // Iniciamos sesión explícitamente en el esquema de cookies para persistir el estado en AJAX
                httpContext.SignInAsync("CookieAuth", authResult.Principal).GetAwaiter().GetResult();
            }

            // 2. ASIGNAR EL USUARIO AL CONTEXTO
            // Una vez validado por el handler, asignamos el Principal resultante al User actual
            httpContext.User = authResult.Principal;

            var result = _authorizationService
                .AuthorizeAsync(httpContext.User, null, "Admin")
                .GetAwaiter()
                .GetResult();

            return result.Succeeded;
        }

    }
}
