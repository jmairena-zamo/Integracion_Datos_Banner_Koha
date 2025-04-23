using ApiBase.Constant;
using ApiBase.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Newtonsoft.Json;

namespace ApiBase.Auth.Authorization
{
    public class CustomAuthorizationMiddleware : IAuthorizationMiddlewareResultHandler
    {
        private readonly AuthorizationMiddlewareResultHandler defaultHandler = new();
        public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy, PolicyAuthorizationResult authorizeResult)
        {
            if (authorizeResult.Forbidden)
            {
                AuthorizationFailureReason? authorizationFailureReason = authorizeResult.AuthorizationFailure?.FailureReasons.FirstOrDefault();
                string? message = authorizationFailureReason?.Message;
                context.Response.Headers.Append("Content-Type", "application/json");
                context.Response.StatusCode = 403;
                await context.Response.WriteAsync(JsonConvert.SerializeObject(
                    new ResponseModel(StatusCodes.Status403Forbidden, ReplyMessages.accessDenied, message))
                    );
                return;
            }

            await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }
}
