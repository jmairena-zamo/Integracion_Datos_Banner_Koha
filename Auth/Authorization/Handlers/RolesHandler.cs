using ApiBase.Auth.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace ApiBase.Auth.Authorization.Handlers
{
    public class RolesHandler : AuthorizationHandler<RolesRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesRequirement requirement)
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

            //VALIDAR QUE EL TOKEN CONTENGA EL CLAIM DE "ROLES"
            bool hasClaim = context.User.Claims.Any(x => x.Type == "roles");
            if (!hasClaim)
            {
                context.Fail(new AuthorizationFailureReason(this, "El usuario no tiene roles asignados"));
                return Task.CompletedTask;
            }

            //VALIDAR QUE EL TOKEN CONTENGA ALGUNO DE LOS ROLES REQUERIDOS
            var userRoles = context.User.Claims.Where(c => c.Type == "roles");
            string[] allowedRoles = requirement.Roles;
            bool hasAllowedRole = userRoles.Any(x => allowedRoles.Contains(x.Value)) || allowedRoles.Length == 0;
            if (!hasAllowedRole)
            {
                context.Fail(new AuthorizationFailureReason(this, "El usuario no cuenta con alguno de los roles requeridos"));
                return Task.CompletedTask;
            }

            //EL REQUERIMIENTO FUE CUMPLIDO CON ÉXITO
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}