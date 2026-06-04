using Integracion_Datos_Banner_Koha.Models.DTOs;

namespace Integracion_Datos_Banner_Koha.Services.Integracion_Banner_Koha.Interfaces
{
    public interface IKohaSyncService
    {
        Task<List<KohaPatronDto>> ObtenerPatronsActivosKohaAsync();
        Task<bool> CrearPatronKohaAsync(BannerStudentDto estudiante);
    }
}
