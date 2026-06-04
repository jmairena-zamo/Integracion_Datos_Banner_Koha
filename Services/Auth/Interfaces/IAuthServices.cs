using Integracion_Datos_Banner_Koha.Models;

namespace Integracion_Datos_Banner_Koha.Services.Auth.Interfaces
{
    public interface IAuthServices
    {
        Task<ResponseModel> AuthenticationRequestPost(string path, object body);
    }
}
