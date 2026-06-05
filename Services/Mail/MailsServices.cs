using Integracion_Datos_Banner_Koha.Models.DTOs;
using Integracion_Datos_Banner_Koha.Models.DTOs.Mails;
using Integracion_Datos_Banner_Koha.Models.DTOs.Response;
using Integracion_Datos_Banner_Koha.Services.Mail.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Integracion_Datos_Banner_Koha.Services.Mail
{
    public class MailsServices : IMailsServices
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public MailsServices(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<HttpResponseMessage>> SendEmailAsync(string endpoint, MailsModel mailsModel)
        {
            try
            {
                string urlBase = _configuration.GetSection("ZamoMails:Url").Value ?? "https://api.zamorano.edu";
                string? profileName = _configuration.GetSection("ZamoMails:Profile").Value;
                string? zamoMailsApiKey = _configuration.GetSection("ZamoMails:ApiKey").Value;

                var uriBuilder = new UriBuilder(urlBase + endpoint);
                var jsonContent = new StringContent(JsonSerializer.Serialize(mailsModel), Encoding.UTF8, "application/json");

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("ProfileName", profileName);
                _httpClient.DefaultRequestHeaders.Add("x-api-key", zamoMailsApiKey);

                HttpResponseMessage response = await _httpClient.PostAsync(uriBuilder.Uri, jsonContent);

                if (!response.IsSuccessStatusCode)
                {
                    return ApiResponse<HttpResponseMessage>.Failure(
                        (int)response.StatusCode,
                        new { Message = "La API de Zamorano rechazó el envío del correo." }
                    );
                }

                return ApiResponse<HttpResponseMessage>.Success((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return ApiResponse<HttpResponseMessage>.Failure(
                    500,
                    new { Message = "Error interno del servidor al procesar el envío.", Error = ex.Message }
                );
            }
        }
    }
}
