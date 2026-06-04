namespace Integracion_Datos_Banner_Koha.Services.Integracion_Banner_Koha.Interfaces
{
    public interface IAccionesService
    {
        Task<object> AnalizarYCruzarDatosAsync(string usuarioCreador, string host);
    }
}
