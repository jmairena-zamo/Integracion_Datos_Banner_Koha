using ApiBase.Auth.DTO;
using ApiBase.Constant;
using ApiBase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace ApiBase.Auth.Authorization.Attributes
{
    public class ApiKeyFilter : IAsyncAuthorizationFilter
    {
        private readonly IConfiguration config;
        private readonly string[] _roles;
        private const string headerKeyName = "X-API-Key";
        private string failReason = "";

        public ApiKeyFilter(IConfiguration config, string[] Roles)
        {
            this.config = config;
            _roles = Roles;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            //Verificar ingreso de ApiKey en los Headers
            string? apiKey = context.HttpContext.Request.Headers[headerKeyName].ToString();
            if (apiKey == null || apiKey == "")
            {
                failReason = "No se ha proporcionado un ApiKey";
                context.Result = JsonResponse(context.HttpContext, failReason);
                return;
            }

            //Verificar si el proyecto cuenta con un código de API
            string? codigoApiProject = config.GetSection("CodigoApiProject").Value;
            if (codigoApiProject == null || codigoApiProject == "")
            {
                failReason = "No se ha proporcionado un código de API";
                context.Result = JsonResponse(context.HttpContext, failReason);
                return;
            }

            //Verificar si el proyecto cuenta con la url de AUTH
            string? authUrl = config.GetSection("AuthUrl").Value;
            if (authUrl == null || authUrl == "")
            {
                failReason = "No se ha proporcionado la url de auth";
                context.Result = JsonResponse(context.HttpContext, failReason);
                return;
            }

            //Validar si el ApiKey es válido
            ApiKeyValidationDto apiKeyModel = new() { ApiKey = apiKey, Roles = _roles };
            HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Add("CodigoApiProject", codigoApiProject);
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(authUrl + "/auth/v1/actionsApiKey/validate", apiKeyModel);
            if (!response.IsSuccessStatusCode)
            {
                failReason = "ApiKey no válido";
                string body = await response.Content.ReadAsStringAsync();
                ResponseModel result = JsonConvert.DeserializeObject<ResponseModel>(body)!;
                context.Result = JsonResponse(context.HttpContext, result.Message);
                return;
            }
        }

        private JsonResult JsonResponse(HttpContext context, string message)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.Headers.Add("Content-Type", "application/json");
            return new JsonResult(new ResponseModel(StatusCodes.Status403Forbidden, ReplyMessages.accessDenied, message));
        }
    }
}
