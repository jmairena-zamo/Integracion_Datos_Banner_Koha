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