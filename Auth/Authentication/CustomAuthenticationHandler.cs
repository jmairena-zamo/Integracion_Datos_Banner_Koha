using Integracion_Datos_Banner_Koha.Auth.DTO;
using Integracion_Datos_Banner_Koha.Constant;
using Integracion_Datos_Banner_Koha.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Integracion_Datos_Banner_Koha.Auth.Authentication
{
    public class CustomAuthenticationHandler : AuthenticationHandler<CustomAuthenticationOptions>
    {
        private readonly IConfiguration config;
        private string failReason = "";

        public CustomAuthenticationHandler(
            IOptionsMonitor<CustomAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, //Obligatorios por AuthenticationHandler
            IConfiguration config)
        : base(options, logger, encoder)
        {
            this.config = config;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //VERIFICAR INGRESO DE TOKEN EN LOS HEADERS
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            if (token == null || token == "")
            {
                failReason = "No se ha proporcionado un token";
                return AuthenticateResult.Fail(failReason);
            }

            //VERIFICAR SI EL PROYECTO CUENTA CON UN CÓDIGO DE API
            string? codigoApiProject = config.GetSection("CodigoApiProject").Value;
            if (codigoApiProject == null || codigoApiProject == "")
            {
                failReason = "No se ha proporcionado un código de API";
                return AuthenticateResult.Fail(failReason);
            }

            //VERIFICAR SI EL PROYECTO CUENTA CON LA URL DE AUTH
            string? authUrl = config.GetSection("AuthUrl").Value;
            if (authUrl == null || authUrl == "")
            {
                failReason = "No se ha proporcionado la url de auth";
                return AuthenticateResult.Fail(failReason);
            }

            //VALIDAR SI EL TOKEN ES VÁLIDO
            TokenValidationDto tokenModel = new TokenValidationDto { Token = token };
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("CodigoApiProject", codigoApiProject);
            HttpResponseMessage response = await httpClient.PostAsJsonAsync(authUrl + "/auth/v1/actionsToken/validate", tokenModel);
            if (!response.IsSuccessStatusCode)
            {
                failReason = "Token no válido";
                return AuthenticateResult.Fail(failReason);
            }

            //LA VERIFICACIÓN DEL TOKEN FUE EXITOSA
            ClaimsPrincipal principal = GetPrincipal(token);
            return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            //RETORNA UNA REPUESTA PERSONALIZADA
            string errorMessage = JsonConvert.SerializeObject(new ResponseModel(StatusCodes.Status401Unauthorized, ReplyMessages.invalidToken, failReason));
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
