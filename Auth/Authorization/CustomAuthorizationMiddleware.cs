using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Integracion_Datos_Banner_Koha.Constant;
using Integracion_Datos_Banner_Koha.Models;

namespace Integracion_Datos_Banner_Koha.Auth.Authorization
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
                context.Response.Headers["Content-Type"] = "application/json";
                context.Response.StatusCode = 403;
                JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
                serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                ResponseModel response = new ResponseModel(StatusCodes.Status403Forbidden, ReplyMessages.accessDenied, message);
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response, serializerSettings));
                return;
            }

            await defaultHandler.HandleAsync(next, context, policy, authorizeResult);
        }
    }
}
