//Título del Programa: BannerKoha
//Autor: José Anibal Mairena Diaz
//Fecha de Creación:  19 / 05 / 2026
//Lenguaje: C#
//Versión: v1.0
/*Propósito: En este apartado se generan las tareas que realizará el servicio
  de AccionesService para la integración de Banner con Koha.*/
//Responsable de Mantenimiento: José Anibal Mairena Diaz

namespace Integracion_Datos_Banner_Koha.Services.Integracion_Banner_Koha.Interfaces
{
    public interface IAccionesService
    {
        Task<object> AnalizarYCruzarDatosAsync(string usuarioCreador, string host);
    }
}
