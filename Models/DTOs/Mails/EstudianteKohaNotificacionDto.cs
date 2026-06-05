//Título del Programa: BannerKoha
//Autor: José Anibal Mairena Diaz
//Fecha de Creación:  19 / 05 / 2026
//Lenguaje: C#
//Versión: v1.0
/*Propósito: En este apartado se guardan los daros de los estudiantes a los que
  se les crea el perfil en Koha.*/
//Responsable de Mantenimiento: José Anibal Mairena Diaz

namespace Integracion_Datos_Banner_Koha.Models.DTOs.Mails
{
    public class EstudianteKohaNotificacionDto
    {
        public string BannerId { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string CorreoInstitucional { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public bool EsExito { get; set; }
        public DateTime FechaProceso { get; set; } = DateTime.Now;
    }
}
