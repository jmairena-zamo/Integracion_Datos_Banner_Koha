using ApiBase.Auth.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace ApiBase.Auth.Authorization.Handlers
{
    public class RolesHandler : AuthorizationHandler<RolesRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, RolesRequirement requirement)
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

            //Validar que el token contenga el claim de "roles"
            bool hasClaim = context.User.Claims.Any(x => x.Type == "roles");
            if (!hasClaim)
            {
                context.Fail(new AuthorizationFailureReason(this, "El usuario no tiene roles asignados"));
                return Task.CompletedTask;
            }

            //Validar que el token contenga alguno de los roles requeridos
            var userRoles = context.User.Claims.Where(c => c.Type == "roles");
            string[] allowedRoles = requirement.Roles;
            bool hasAllowedRole = userRoles.Any(x => allowedRoles.Contains(x.Value)) || allowedRoles.Length==0;
            if (!hasAllowedRole)
            {
                context.Fail(new AuthorizationFailureReason(this, "El usuario no cuenta con alguno de los roles requeridos"));
                return Task.CompletedTask;
            }

            //El requerimiento fue cumplido con éxito
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}