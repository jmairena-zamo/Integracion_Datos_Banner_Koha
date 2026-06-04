using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Integracion_Datos_Banner_Koha.Models;
using Integracion_Datos_Banner_Koha.Models.DTOs.Auth;
using Integracion_Datos_Banner_Koha.Services.Auth.Interfaces;
using System.IdentityModel.Tokens.Jwt;

namespace Integracion_Datos_Banner_Koha.Controllers.Auth.v1
{
    [Route("auth")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "Auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices authServices;

        public AuthController(IAuthServices authServices)
        {
            this.authServices = authServices;
        }

        [Route("token")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Token([FromBody] UserLoginInputDto userLogin)
        {
            ResponseModel response = await authServices.AuthenticationRequestPost("/auth/v1/ad/anyUser", userLogin);

            if (response.Status >= 400)
            {
                response = await authServices.AuthenticationRequestPost("/auth/v1/external/user", userLogin);
            }
            if (response.Status == 200 && response.Response != null)
            {
                var jsonResponse = JObject.FromObject(response.Response);
                string jwtToken = jsonResponse["token"]?.ToString();
                var handler = new JwtSecurityTokenHandler();
                var jwtDecoded = handler.ReadJwtToken(jwtToken);
                var expirationDate = jwtDecoded.ValidTo;
                // Mandamos la cookie cruda
                Response.Cookies.Append("token", jwtToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = expirationDate // La cookie morirá exactamente con el token
                });
            }

            return StatusCode(response.Status, response);
        }

        [Route("refreshToken")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> refreshToken([FromBody] RefreshTokenInputDto refreshTokenModel)
        {
            ResponseModel response = await authServices.AuthenticationRequestPost("/auth/v1/actionsToken/generateRefreshToken", refreshTokenModel);
            return StatusCode(response.Status, response);
        }
    }
}
