using ApiBase.Auth.Authorization.Attributes;
using ApiBase.Models;
using ApiBase.Models.DTOs.Input;
using ApiBase.Services.Interfaces;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace ApiBase.Controllers.v1
{
    [Route("v{version:apiVersion}/ejemplo")]
    [ApiController]
    public class EjemploController : ControllerBase
    {
        private IEjemploService ejemploServices;
        public EjemploController(IEjemploService ejemploServices)
        {
            this.ejemploServices = ejemploServices;
        }

        [HttpGet]
        [ApiVersion("1.0")]
        [ApiKey("ADMINISTRADOR")]
        public async Task<IActionResult> getEstudiantes()
        {
            ResponseModel response = await ejemploServices.getEstudiantes();
            return StatusCode(response.Status, response);
        }

        [HttpGet("getById/{id}")]
        [ApiVersion("1.0")]
        public async Task<IActionResult> getEstudianteById(int id)
        {
            ResponseModel response = await ejemploServices.getEstudianteById(id);
            return StatusCode(response.Status, response);
        }

        [HttpPost]
        [ApiVersion("1.0")]
        [Authorize]
        public async Task<IActionResult> createEstudiante(EjemploInputDto ejemploInputDto)
        {
            string user = User.Claims.Where(a => a.Type == "username").FirstOrDefault()?.Value!;
            ResponseModel response = await ejemploServices.createEstudiante(ejemploInputDto, user);
            return StatusCode(response.Status, response);
        }

        [HttpPut("{id}")]
        [ApiVersion("1.0")]
        [Authorize]
        public async Task<IActionResult> updateEstudiante(int id, [FromBody] EjemploInputDto ejemploInputDto)
        {
            string user = User.Claims.Where(a => a.Type == "username").FirstOrDefault()?.Value!;
            ResponseModel response = await ejemploServices.updateEstudiante(id, ejemploInputDto, user);
            return StatusCode(response.Status, response);
        }

        [HttpPatch("{id}")]
        [ApiVersion("1.0")]
        [Authorize]
        public async Task<ActionResult> patchEstudiante(int id, [FromBody] JsonPatchDocument<EjemploInputDto> patchDoc)
        {
            string user = User.Claims.Where(a => a.Type == "username").FirstOrDefault()?.Value!;
            ResponseModel response = await ejemploServices.patchEstudiante(id, patchDoc, user);
            return StatusCode(response.Status, response);
        }

        [HttpDelete("{id}")]
        [ApiVersion("1.0")]
        [Authorize]
        public async Task<IActionResult> deleteEstudiante(int id)
        {
            string user = User.Claims.Where(a => a.Type == "username").FirstOrDefault()?.Value!;
            ResponseModel response = await ejemploServices.deleteEstudiante(id, user);
            return StatusCode(response.Status, response);
        }
    }
}
