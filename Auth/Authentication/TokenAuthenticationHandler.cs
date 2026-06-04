using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Integracion_Datos_Banner_Koha.Auth.DTO;
using Integracion_Datos_Banner_Koha.Constant;
using Integracion_Datos_Banner_Koha.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace Integracion_Datos_Banner_Koha.Auth.Authentication
{
    public class TokenAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IConfiguration config;
        private string failReason = "";

        public TokenAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, //Obligatorios por AuthenticationHandler
            IConfiguration config)
        : base(options, logger, encoder)
        {
            this.config = config;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //Verificar ingreso de token en los Headers
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                Request.Cookies.TryGetValue("token", out token);
            }
            if (token == null || token == "")
            {
                failReason = "No se ha proporcionado un token";
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

            //Validar si el token es válido
            TokenValidationDto tokenModel = new TokenValidationDto { Token = token };
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("CodigoApiProject", codigoApiProject);
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(authUrl + "/auth/v1/actionsToken/validate", tokenModel);
            if (!response.IsSuccessStatusCode)
            {
                failReason = "Token no válido";
                return AuthenticateResult.Fail(failReason);
            }

            //La verificación del token fue exitosa
            var json = await response.Content.ReadAsStringAsync();
            token = JsonDocument.Parse(json).RootElement.GetProperty("response").GetProperty("zamoranoToken").GetString() ?? "";

            ClaimsPrincipal principal = GetPrincipal(token);
            return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            // Retorna una respuesta personalizada
            JsonSerializerSettings serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            ResponseModel response = new ResponseModel(StatusCodes.Status401Unauthorized, ReplyMessages.invalidToken, failReason);
            string errorMessage = JsonConvert.SerializeObject(response, serializerSettings);
            Response.Headers.Append("Content-Type", "application/json");
            Response.StatusCode = 401;
            await Response.WriteAsync(errorMessage);
        }

        private ClaimsPrincipal GetPrincipal(string Token)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken? token = handler.ReadToken(Token) as JwtSecurityToken;
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(token!.Claims, "Token");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            return claimsPrincipal;
        }
    }
}
