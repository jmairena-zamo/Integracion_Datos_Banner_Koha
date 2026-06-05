//Título del Programa: BannerKoha
//Autor: José Anibal Mairena Diaz
//Fecha de Creación:  19 / 05 / 2026
//Lenguaje: C#
//Versión: v1.0
/*Propósito: En este apartado se obtiene la información de los estudiantes con perfil
  en la plataforma de Koha con el fin de poder integrar datos de Banner para compararlos 
  con los perfiles y, en todo caso, crear nuevos usuarios*/
//Responsable de Mantenimiento: José Anibal Mairena Diaz

using Integracion_Datos_Banner_Koha.Models.DTOs;
using Integracion_Datos_Banner_Koha.Services.Integracion_Banner_Koha.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text.Json;
using System.Threading.Tasks;

namespace Integracion_Datos_Banner_Koha.Services.Integracion_Banner_Koha
{
    public class KohaSyncService : IKohaSyncService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public KohaSyncService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<KohaPatronDto>> ObtenerPatronsActivosKohaAsync()
        {
            string token = await ObtenerTokenKohaAsync();
            var kohaUrl = _configuration["KohaSettings:BaseUrl"]?.TrimEnd('/');
            var version = _configuration["KohaSettings:ApiVersion"];
            var patronUrl = _configuration["KohaSettings:PatronPath"]?.TrimStart('/');

            if (string.IsNullOrEmpty(kohaUrl) || string.IsNullOrEmpty(version) || string.IsNullOrEmpty(patronUrl))
            {
                throw new InvalidOperationException("La configuración de conexión a Koha no está definida.");
            }

            var endpointPath = patronUrl.Replace("{version}", version);
            var urlPatrons = $"{kohaUrl}/{endpointPath}";

            var request = new HttpRequestMessage(HttpMethod.Get, urlPatrons);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new Exception($"La API de Koha falló. Status: {response.StatusCode}. Detalle: {errorContent}");
            }

            var responseString = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(responseString);
            var root = doc.RootElement;

            if (root.ValueKind == JsonValueKind.Array)
            {
                var opciones = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var todosLosPatrons = JsonSerializer.Deserialize<List<KohaPatronDto>>(root.GetRawText(), opciones);

                return todosLosPatrons ?? new List<KohaPatronDto>();
            }
            else
            {
                throw new Exception($"Koha no retornó un arreglo. Respuesta del servidor: {responseString}");
            }
        }

        public async Task<bool> CrearPatronKohaAsync(BannerStudentDto estudiante)
        {
            string token = await ObtenerTokenKohaAsync();

            var kohaUrl = _configuration["KohaSettings:BaseUrl"]?.TrimEnd('/');
            var version = _configuration["KohaSettings:ApiVersion"];

            string kohaApiUrl = $"{kohaUrl}/api/{version}/patrons";

            var request = new HttpRequestMessage(HttpMethod.Post, kohaApiUrl);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var kohaPayload = new
            {
                cardnumber = estudiante.BannerId,
                firstname = estudiante.Nombre,
                surname = estudiante.Apellido,
                email = estudiante.Correo,
                category_id = "ST",
                library_id = "01"
            };

            var jsonContent = JsonSerializer.Serialize(kohaPayload);
            request.Content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.SendAsync(request);

            return response.IsSuccessStatusCode;
        }

        private async Task<string> ObtenerTokenKohaAsync()
        {
            var kohaUrl = _configuration["KohaSettings:BaseUrl"]?.TrimEnd('/');
            var version = _configuration["KohaSettings:ApiVersion"];
            var tokenUrl = _configuration["KohaSettings:TokenPath"]?.TrimStart('/');
            var grant_type = _configuration["KohaSettings:GrandType"];
            var client_id = _configuration["KohaSettings:ClientId"];
            var client_secret = _configuration["KohaSettings:ClientSecret"];

            if (string.IsNullOrEmpty(kohaUrl) || string.IsNullOrEmpty(version) || string.IsNullOrEmpty(tokenUrl) || string.IsNullOrEmpty(grant_type) || string.IsNullOrEmpty(client_id) || string.IsNullOrEmpty(client_secret))
            {
                throw new InvalidOperationException("La configuración de conexión a Koha no está definida.");
            }

            var endpointPath = tokenUrl.Replace("{version}", version);
            var urlToken = $"{kohaUrl}/{endpointPath}";

            var parametrosFormulario = new Dictionary<string, string>
            {
                {"grant_type", grant_type },
                { "client_id", client_id },
                { "client_secret", client_secret }
            };

            var requestContent = new FormUrlEncodedContent(parametrosFormulario);

            var response = await _httpClient.PostAsync(urlToken, requestContent);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new Exception($"No se pudo generar el Token de Koha. Status: {response.StatusCode}. Detalle: {errorBody}");
            }

            var responseString = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseString);
            var root = doc.RootElement;

            if (root.TryGetProperty("access_token", out JsonElement accessTokenProp))
            {
                string? token = accessTokenProp.GetString();
                if (!string.IsNullOrEmpty(token))
                {
                    return token;
                }
            }

            throw new Exception("La respuesta de autenticación de Koha fue exitosa, pero no contenía la propiedad 'access_token'.");
        }
    }
}
