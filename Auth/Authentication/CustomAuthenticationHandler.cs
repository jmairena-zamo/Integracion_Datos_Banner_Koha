using ApiBase.Auth.DTO;
using ApiBase.Constant;
using ApiBase.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace ApiBase.Auth.Authentication
{
    public class CustomAuthenticationHandler : AuthenticationHandler<CustomAuthenticationOptions>
    {
        private readonly IConfiguration config;
        private string failReason = "";

        public CustomAuthenticationHandler(
            IOptionsMonitor<CustomAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, //Obligatorios por "base"
            IConfiguration config)
        : base(options, logger, encoder, clock)
        {
            this.config = config;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            //Verificar ingreso de token en los Headers
            string token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
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
            ClaimsPrincipal principal = GetPrincipal(token);
            return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            //Retorna una repuesta personalizada
            string errorMessage = JsonConvert.SerializeObject(new ResponseModel(StatusCodes.Status401Unauthorized, ReplyMessages.invalidToken, failReason));
            Response.Headers.Add("Content-Type", "application/json");
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
