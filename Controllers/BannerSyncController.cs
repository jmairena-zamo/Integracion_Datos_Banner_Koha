//Título del Programa: BannerKoha
//Autor: José Anibal Mairena Diaz
//Fecha de Creación:  19 / 05 / 2026
//Lenguaje: C#
//Versión: v1.0
/*Propósito: En este apartado se controlan los servicios de BannerSyncServices
  para así ser utilizado de manera manual, este no es obligatorio usarlo, ya que
  este proyecto es un job que se generará automáticamente.*/
//Responsable de Mantenimiento: José Anibal Mairena Diaz

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Integracion_Datos_Banner_Koha.Services;
using Integracion_Datos_Banner_Koha.Services.Integracion_Banner_Koha.Interfaces;

namespace Integracion_Datos_Banner_Koha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BannerSyncController : ControllerBase
    {
        private readonly IBannerSyncService _syncService;

        public BannerSyncController(IBannerSyncService syncService)
        {
            _syncService = syncService;
        }

        [HttpGet("BannerInfo")]
        public async Task<IActionResult> EjecutarSincronizacion()
        {
            try
            {
                var resultado = await _syncService.EjecutarSincronizacionAsync();
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    mensaje = "Ocurrió un error crítico durante la sincronización.",
                    error = ex.Message
                });
            }
        }
    }
}