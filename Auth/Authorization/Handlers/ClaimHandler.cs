using ApiBase.Auth.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;

namespace ApiBase.Auth.Authorization.Handlers
{
    public class ClaimHandler : AuthorizationHandler<ClaimRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ClaimRequirement requirement)
        {
            //VALIDAR IDENTIDAD
            if (context.User.Identity == null)
            {
                context.Fail(new AuthorizationFailureReason(this, "El usuario no cuenta con una identidad"));
                return Task.CompletedTask;
            }

            //VALIDAR USUARIO AUTENTICADO
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail(new AuthorizationFailureReason(this, "El usuario no está autenticado"));
                return Task.CompletedTask;
            }

            //VALIDAR QUE EL TOKEN CONTENGA EL CLAIM ESPECIFICADO
            string requiredClaim = requirement.ClaimType;
            bool hasClaim = context.User.Claims.Any(x => x.Type == requiredClaim);
            if (!hasClaim)
            {
                context.Fail(new AuthorizationFailureReason(this, $"El usuario no contiene el claim {requiredClaim}"));
                return Task.CompletedTask;
            }

            //EL REQUERIMIENTO FUE CUMPLIDO CON ÉXITO
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

    }
}
