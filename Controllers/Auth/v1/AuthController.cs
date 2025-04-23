using ApiBase.Models;
using ApiBase.Models.DTOs.Auth.Input;
using ApiBase.Services.Auth.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApiBase.Controllers.Auth.v1
{
    [Route("auth")]
    [ApiController]

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
