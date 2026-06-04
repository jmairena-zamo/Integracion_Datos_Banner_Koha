using auth.Models.DTOs.Input;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Integracion_Datos_Banner_Koha.Constant;
using Integracion_Datos_Banner_Koha.Models;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Integracion_Datos_Banner_Koha.Auth.Authentication
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IConfiguration config;
        private string failReason = "";

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, //Obligatorios por AuthenticationHandler
            IConfiguration config)
        : base(options, logger, encoder)
        {
            this.config = config;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //Verificar ingreso de ApiKey en los Headers
            if (!Request.Headers.TryGetValue("x-api-key", out var apiKey))
            {
                failReason = "No se ha proporcionado un ApiKey";
                return AuthenticateResult.Fail(failReason);
            }

            //Verificar si el proyecto cuenta con un código de API
            string? codigoApiProject = config.GetSection("CodigoApiProject").Value;
            if (codigoApiProject == null || codigoApiProject == "")
            {
                failReason = "No se ha proporcionado un código de API";
                return AuthenticateResult.Fail(failReason);
            }

            //Verificar si el proyecto cuenta con la url de AUTH
            string? authUrl = config.GetSection("AuthUrl").Value;
            if (authUrl == null || authUrl == "")
            {
                failReason = "No se ha proporcionado la url de auth";
                return AuthenticateResult.Fail(failReason);
            }

            //Validar si el ApiKey es válido
            ApiKeyValidationDto apiKeyModel = new ApiKeyValidationDto { ApiKey = apiKey };
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("CodigoApiProject", codigoApiProject);
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(authUrl + "/auth/v2/actionsApiKey/validate", apiKeyModel);
            if (!response.IsSuccessStatusCode)
            {
                failReason = "ApiKey no válido";
                return AuthenticateResult.Fail(failReason);
            }

            //La verificación del ApiKey fue exitosa
            var json = await response.Content.ReadAsStringAsync();
            var rolesElement = JsonDocument.Parse(json).RootElement.GetProperty("response").GetProperty("roles");
            var claims = new List<Claim>();

            foreach (var role in rolesElement.EnumerateArray())
            {
                string roleName = role.GetString() ?? "";
                if (!string.IsNullOrEmpty(roleName))
                {
                    claims.Add(new Claim("roles", roleName));
                }
            }

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);

            return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            // Retorna una respuesta personalizada
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            ResponseModel response = new ResponseModel(StatusCodes.Status401Unauthorized, ReplyMessages.invalidApiKey, failReason);
            string errorMessage = JsonConvert.SerializeObject(response, serializerSettings);
            Response.Headers.Append("Content-Type", "application/json");
            Response.StatusCode = 401;
            await Response.WriteAsync(errorMessage);
        }
    }
}
