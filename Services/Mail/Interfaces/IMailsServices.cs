using Integracion_Datos_Banner_Koha.Models.DTOs.Mails;
using Integracion_Datos_Banner_Koha.Models.DTOs.Response;

namespace Integracion_Datos_Banner_Koha.Services.Mail.Interfaces
{
    public interface IMailsServices
    {
        Task<ApiResponse<HttpResponseMessage>> SendEmailAsync(string endpoint, MailsModel mailsModel);
    }
}
