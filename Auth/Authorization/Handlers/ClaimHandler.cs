using Microsoft.AspNetCore.Authorization;
using Integracion_Datos_Banner_Koha.Auth.Authorization.Requirements;

namespace Integracion_Datos_Banner_Koha.Auth.Authorization.Handlers
{
    public class ClaimHandler : AuthorizationHandler<ClaimRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClaimRequirement requirement)
        {
            //Validar identidad
            if (context.User.Identity == null)
            {
                context.Fail(new AuthorizationFailureReason(this, "El usuario no cuenta con una identidad"));
                return Task.CompletedTask;
            }

            //Validar usuario autenticado
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail(new AuthorizationFailureReason(this, "El usuario no está autenticado"));
                return Task.CompletedTask;
            }

            //Validar que el token contenga el claim especificado
            string requiredClaim = requirement.ClaimType;
            bool hasClaim = context.User.Claims.Any(x => x.Type == requiredClaim);
            if (!hasClaim)
            {
                context.Fail(new AuthorizationFailureReason(this, $"El usuario no contiene el claim {requiredClaim}"));
                return Task.CompletedTask;
            }

            //El requerimiento fue cumplido con éxito
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

    }
}
