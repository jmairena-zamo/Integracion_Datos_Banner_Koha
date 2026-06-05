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
