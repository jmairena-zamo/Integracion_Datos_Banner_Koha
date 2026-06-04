using Integracion_Datos_Banner_Koha.Services.Integracion_Banner_Koha.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Integracion_Datos_Banner_Koha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KohaSyncController : ControllerBase
    {
        private readonly IKohaSyncService _kohaService;

        public KohaSyncController(IKohaSyncService kohaService)
        {
            _kohaService = kohaService;
        }

        [HttpGet("KohaInfo")]
        public async Task<IActionResult> ObtenerPatronsActivos()
        {
            try
            {
                var resultado = await _kohaService.ObtenerPatronsActivosKohaAsync();

                return Ok(new
                {
                    totalCount = resultado.Count,
                    items = resultado
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    mensaje = "Ocurrió un error al conectar u obtener los datos de Koha.",
                    error = ex.Message
                });
            }
        }
    }
}
