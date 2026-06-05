//Título del Programa: BannerKoha
//Autor: José Anibal Mairena Diaz
//Fecha de Creación:  19 / 05 / 2026
//Lenguaje: C#
//Versión: v1.0
/*Propósito: En este apartado se controlan los servicios de AccionesServices
  para así ser utilizado de manera manual, este no es obligatorio usarlo, ya que
  este proyecto es un job que se generará automáticamente.*/
//Responsable de Mantenimiento: José Anibal Mairena Diaz

using Hangfire;
using Integracion_Datos_Banner_Koha.Services;
using Integracion_Datos_Banner_Koha.Services.Integracion_Banner_Koha.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace Integracion_Datos_Banner_Koha.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccionesController : ControllerBase
    {
        private readonly IBackgroundJobClient _backgroundJobClient;

        public AccionesController(IBackgroundJobClient backgroundJobClient)
        {
            _backgroundJobClient = backgroundJobClient;
        }

        [HttpGet("Trigger_Job")]
        public IActionResult AnalizarSincronizacion()
        {
            string ipUsuario = Request.Headers["X-Forwarded-For"].FirstOrDefault()
                       ?? HttpContext.Connection.RemoteIpAddress?.ToString()
                       ?? "127.0.0.1";

            string usuario = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                     ?? User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value
                     ?? User.Identity?.Name
                     ?? "Api_Koha";

            _backgroundJobClient.Enqueue<IAccionesService>(x =>
                x.AnalizarYCruzarDatosAsync(usuario, ipUsuario));

            return Accepted(new
            {
                mensaje = "El proceso de sincronización ha sido encolado exitosamente.",
                usuario = usuario,
                ip = ipUsuario,
                detalles = "Monitorea el progreso en /hangfire"
            });
        }
    }
}
