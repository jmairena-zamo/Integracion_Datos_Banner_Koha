//Título del Programa: BannerKoha
//Autor: José Anibal Mairena Diaz
//Fecha de Creación:  19 / 05 / 2026
//Lenguaje: C#
//Versión: v1.0
/*Propósito: En este apartado se generan las tareas que realizará el servicio
  de BannerSyncService para la integración de Banner con Koha.*/
//Responsable de Mantenimiento: José Anibal Mairena Diaz

using Integracion_Datos_Banner_Koha.Models;
using Integracion_Datos_Banner_Koha.Models.DTOs;
using System.Threading.Tasks;

namespace Integracion_Datos_Banner_Koha.Services.Integracion_Banner_Koha.Interfaces
{
    public interface IBannerSyncService
    {
        Task<List<BannerStudentDto>> EjecutarSincronizacionAsync(CancellationToken cancellationToken = default);
    }
}