//Título del Programa: BannerKoha
//Autor: José Anibal Mairena Diaz
//Fecha de Creación:  19 / 05 / 2026
//Lenguaje: C#
//Versión: v1.0
/*Propósito: En este apartado se generan las tareas que realizará el servicio
  de MailsServices para el envío de correos de la integración de Banner con Koha.*/
//Responsable de Mantenimiento: José Anibal Mairena Diaz

using Integracion_Datos_Banner_Koha.Models.DTOs.Mails;
using Integracion_Datos_Banner_Koha.Models.DTOs.Response;

namespace Integracion_Datos_Banner_Koha.Services.Mail.Interfaces
{
    public interface IMailsServices
    {
        Task<ApiResponse<HttpResponseMessage>> SendEmailAsync(string endpoint, MailsModel mailsModel);
    }
}
