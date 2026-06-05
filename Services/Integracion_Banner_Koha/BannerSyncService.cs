//Título del Programa: BannerKoha
//Autor: José Anibal Mairena Diaz
//Fecha de Creación:  19 / 05 / 2026
//Lenguaje: C#
//Versión: v1.0
/*Propósito: En este apartado se obtiene la información de los estudiantes desde Banner
  con el fin de poder integrar datos de Banner para compararlos y, en todo caso, crear 
  nuevos usuarios en la plataforma de Koha*/
//Responsable de Mantenimiento: José Anibal Mairena Diaz

using Integracion_Datos_Banner_Koha.Models;
using Integracion_Datos_Banner_Koha.Models.DTOs;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Integracion_Datos_Banner_Koha.Services.Integracion_Banner_Koha.Interfaces;

namespace Integracion_Datos_Banner_Koha.Services.Integracion_Banner_Koha
{
    public class BannerSyncService : IBannerSyncService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public BannerSyncService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<BannerStudentDto>> EjecutarSincronizacionAsync(CancellationToken cancellationToken = default)
        {
            var url = _configuration["BannerSatelliteSettings:BaseUrl"]?.TrimEnd('/');
            var version = _configuration["BannerSatelliteSettings:ApiVersion"];
            var satellitePath = _configuration["BannerSatelliteSettings:SatellitePath"]?.TrimStart('/');
            var apiKey = _configuration["BannerSatelliteSettings:ApiKey"];

            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(version) || string.IsNullOrEmpty(satellitePath) ||  string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("La configuración de conexión a Banner no está definida.");
            }

            var endpointPath = satellitePath.Replace("{version}", version);
            var urlComplete = $"{url}/{endpointPath}";

            var allStudents = new List<BannerStudentDto>();
            int limit = 1000;
            int offset = 0;
            bool keepReading = true;

            while (keepReading)
            {
                var payloadObj = new Dictionary<string, object>
                {
                    { "limit", limit },
                    { "offset", offset },
                    { "entities", new List<object>
                        {
                            new Dictionary<string, object> { { "name", "SPRIDEN" } },
                            new Dictionary<string, object>
                            {
                                { "name", "SGBSTDN" }, { "joinType", "inner" },
                                { "joinConditions", new List<object> { new Dictionary<string, object> { { "name", "SGBSTDN_PIDM" }, { "entityName", "SGBSTDN" }, { "operator", "=" }, { "expression", "SPRIDEN_PIDM" }, { "expressionEntityName", "SPRIDEN" }, { "dataType", "number" } } } }
                            },
                            new Dictionary<string, object>
                            {
                                { "name", "GOREMAL" }, { "joinType", "inner" },
                                { "joinConditions", new List<object> { new Dictionary<string, object> { { "name", "GOREMAL_PIDM" }, { "entityName", "GOREMAL" }, { "operator", "=" }, { "expression", "SPRIDEN_PIDM" }, { "expressionEntityName", "SPRIDEN" }, { "dataType", "number" } } } }
                            }
                        }
                    },
                    { "selects", new List<object>
                        {
                            new Dictionary<string, object> { { "name", "SPRIDEN_ID" }, { "entityName", "SPRIDEN" }, { "valueType", "COLUMN" }, { "dataType", "string" }, { "alias", "BannerId" } },
                            new Dictionary<string, object> { { "name", "SPRIDEN_FIRST_NAME" }, { "entityName", "SPRIDEN" }, { "valueType", "COLUMN" }, { "dataType", "string" }, { "alias", "P_Nombre" } },
                            new Dictionary<string, object> { { "name", "SPRIDEN_MI" }, { "entityName", "SPRIDEN" }, { "valueType", "COLUMN" }, { "dataType", "string" }, { "alias", "S_Nombre" } },
                            new Dictionary<string, object> { { "name", "SPRIDEN_LAST_NAME" }, { "entityName", "SPRIDEN" }, { "valueType", "COLUMN" }, { "dataType", "string" }, { "alias", "Apellido" } },
                            new Dictionary<string, object> { { "name", "GOREMAL_EMAIL_ADDRESS" }, { "entityName", "GOREMAL" }, { "valueType", "COLUMN" }, { "dataType", "string" }, { "alias", "Correo" } },
                            new Dictionary<string, object> { { "name", "SGBSTDN_STST_CODE" }, { "entityName", "SGBSTDN" }, { "valueType", "COLUMN" }, { "dataType", "string" }, { "alias", "Estado" } },
                            new Dictionary<string, object> { { "name", "SGBSTDN_LEVL_CODE" }, { "entityName", "SGBSTDN" }, { "valueType", "COLUMN" }, { "dataType", "string" }, { "alias", "Grado" } }
                        }
                    },
                    { "criteria", new List<object>
                        {
                            new Dictionary<string, object> { { "condition", "null" }, { "name", "SPRIDEN_CHANGE_IND" }, { "nameValueType", "field" }, { "entityName", "SPRIDEN" }, { "operator", "is" }, { "expression", "null" }, { "expressionType", "literal" }, { "dataType", "string" } },
                            new Dictionary<string, object> { { "condition", "AND" }, { "name", "SGBSTDN_STST_CODE" }, { "nameValueType", "field" }, { "entityName", "SGBSTDN" }, { "operator", "=" }, { "expression", "'AS'" }, { "expressionType", "literal" }, { "dataType", "string" } },
                            new Dictionary<string, object> { { "condition", "AND" }, { "name", "SGBSTDN_LEVL_CODE" }, { "nameValueType", "field" }, { "entityName", "SGBSTDN" }, { "operator", "=" }, { "expression", "'LI'" }, { "expressionType", "literal" }, { "dataType", "string" } },
                            new Dictionary<string, object> { { "condition", "AND" }, { "name", "GOREMAL_EMAL_CODE" }, { "nameValueType", "field" }, { "entityName", "GOREMAL" }, { "operator", "=" }, { "expression", "'INST'" }, { "expressionType", "literal" }, { "dataType", "string" } }
                        }
                    }
                };

                var jsonPayload = JsonSerializer.Serialize(payloadObj);

                var request = new HttpRequestMessage(HttpMethod.Post, urlComplete);
                request.Headers.Add("x-api-key", apiKey);
                request.Headers.ExpectContinue = false;

                var jsonBytes = Encoding.UTF8.GetBytes(jsonPayload);
                var content = new ByteArrayContent(jsonBytes);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                request.Content = content;

                var response = await _httpClient.SendAsync(request, cancellationToken);
                var responseString = await response.Content.ReadAsStringAsync(cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"La API de Banner rechazó la petición en el offset {offset}. Status: {response.StatusCode}. Respuesta: {responseString}");
                }

                using var doc = JsonDocument.Parse(responseString);
                var root = doc.RootElement;

                if (root.TryGetProperty("response", out JsonElement responseProp) &&
            responseProp.TryGetProperty("results", out JsonElement arregloRegistros) &&
            arregloRegistros.ValueKind == JsonValueKind.Array)
                {
                    var opcionesDeserializacion = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var estudiantesPaginaRaw = JsonSerializer.Deserialize<List<BannerRawStudentResponse>>(
                        arregloRegistros.GetRawText(),
                        opcionesDeserializacion
                    );

                    if (estudiantesPaginaRaw != null && estudiantesPaginaRaw.Count > 0)
                    {
                        var estudiantesMapeados = estudiantesPaginaRaw.Select(raw => new BannerStudentDto
                        {
                            BannerId = raw.BannerId,
                            Nombre = $"{raw.P_Nombre} {raw.S_Nombre}".Trim(),
                            Apellido = raw.Apellido?.Replace("*", " "),
                            Correo = raw.Correo,
                            Estado = raw.Estado,
                            Grado = raw.Grado
                        }).ToList();

                        allStudents.AddRange(estudiantesMapeados);

                        if (estudiantesPaginaRaw.Count < limit)
                        {
                            keepReading = false;
                        }
                        else
                        {
                            offset += limit;
                        }
                    }
                    else
                    {
                        keepReading = false;
                    }
                }
                else
                {
                    throw new Exception($"Estructura JSON inválida recibida de Banner en el offset {offset}. No se encontró 'response.results'.");
                }
            }

            return allStudents;
        }

        private class BannerRawStudentResponse
        {
            public string? BannerId { get; set; }
            public string? P_Nombre { get; set; }
            public string? S_Nombre { get; set; }
            public string? Apellido { get; set; }
            public string? Correo { get; set; }
            public string? Estado { get; set; }
            public string? Grado { get; set; }
        }
    }
}