//Título del Programa: BannerKoha
//Autor: José Anibal Mairena Diaz
//Fecha de Creación:  19 / 05 / 2026
//Lenguaje: C#
//Versión: v1.0
/*Propósito: En este apartado se guardan los datos para el envío 
  de correos cn los detalles de los estudiantes creados en Koha.*/
//Responsable de Mantenimiento: José Anibal Mairena Diaz

using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace Integracion_Datos_Banner_Koha.Models.DTOs.Mails
{
    public class MailsModel
    {
        [Required]
        public string? To { get; set; }

        public string? Copy { get; set; }

        public string? Bcc { get; set; }
        [Required]
        public string? Subject { get; set; }
        [Required]
        public string? Body { get; set; }

        public MailPriority Priority { get; set; } = MailPriority.Normal;
    }
}
