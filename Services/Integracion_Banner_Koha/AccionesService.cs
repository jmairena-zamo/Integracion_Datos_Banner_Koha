//Título del Programa: BannerKoha
//Autor: José Anibal Mairena Diaz
//Fecha de Creación:  19 / 05 / 2026
//Lenguaje: C#
//Versión: v1.0
/*Propósito: En este apartado se utilizan los servicios de sincronización de Banner y Koha para analizar y cruzar datos,
  identificar estudiantes nuevos, crear perfiles en Koha, registrar logs de las operaciones y enviar notificaciones por correo
  electrónico con los resultados del proceso. Además, se implementa una captura de errores global para manejar cualquier excepción
  crítica que pueda surgir durante la ejecución del proceso, enviando alertas detalladas al equipo responsable.*/
//Responsable de Mantenimiento: José Anibal Mairena Diaz

using Hangfire;
using Integracion_Datos_Banner_Koha.Context;
using Integracion_Datos_Banner_Koha.Models.DB.Biblioteca;
using Integracion_Datos_Banner_Koha.Models.DTOs;
using Integracion_Datos_Banner_Koha.Models.DTOs.Mails;
using Integracion_Datos_Banner_Koha.Services.Integracion_Banner_Koha.Interfaces;
using Integracion_Datos_Banner_Koha.Services.Mail.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Integracion_Datos_Banner_Koha.Services.Integracion_Banner_Koha
{
    public class AccionesService : IAccionesService
    {
        private readonly IBannerSyncService _bannerService;
        private readonly IKohaSyncService _kohaService;
        private readonly BibliotecaContext _context;
        private readonly IMailsServices _mailsService;
        private readonly IConfiguration _configuration;

        public AccionesService(IBannerSyncService bannerService, IKohaSyncService kohaService, BibliotecaContext context, IMailsServices mailsServices, IConfiguration configuration)
        {
            _bannerService = bannerService;
            _kohaService = kohaService;
            _context = context;
            _mailsService = mailsServices;
            _configuration = configuration;
        }

        public async Task<object> AnalizarYCruzarDatosAsync(string usuarioCreador, string host)
        {
            try
            {
                Task<List<BannerStudentDto>> tareaBanner = _bannerService.EjecutarSincronizacionAsync();
                Task<List<KohaPatronDto>> tareaKoha = _kohaService.ObtenerPatronsActivosKohaAsync();

                await Task.WhenAll(tareaBanner, tareaKoha);

                List<BannerStudentDto> students = await tareaBanner;
                List<KohaPatronDto> usuariosKoha = await tareaKoha;

                if (students == null) students = new List<BannerStudentDto>();
                if (usuariosKoha == null) usuariosKoha = new List<KohaPatronDto>();

                var cardnumbersKohaSet = new HashSet<string>(
                    usuariosKoha
                        .Where(k => !string.IsNullOrEmpty(k.Cardnumber))
                        .Select(k => k.Cardnumber!.Trim()),
                    StringComparer.OrdinalIgnoreCase
                );

                var estudiantesNuevosAInsertar = new List<BannerStudentDto>();
                var estudiantesYaExistentes = new List<BannerStudentDto>();
                var reporteFinalKoha = new List<EstudianteKohaNotificacionDto>();

                foreach (var estudiante in students)
                {
                    if (estudiante == null || string.IsNullOrEmpty(estudiante.BannerId)) continue;

                    string idAEvaluar = estudiante.BannerId.Trim();

                    if (cardnumbersKohaSet.Contains(idAEvaluar))
                    {
                        estudiantesYaExistentes.Add(estudiante);
                    }
                    else
                    {
                        estudiantesNuevosAInsertar.Add(estudiante);
                    }
                }

                int kohaPostExitosos = 0;
                int kohaPostFallidos = 0;

                if (estudiantesNuevosAInsertar.Any())
                {
                    foreach (var est in estudiantesNuevosAInsertar)
                    {
                        bool seCreoEnKoha = false;
                        try
                        {
                            seCreoEnKoha = await _kohaService.CrearPatronKohaAsync(est);
                        }
                        catch (Exception)
                        {
                            seCreoEnKoha = false;
                        }

                        if (seCreoEnKoha)
                        {
                            kohaPostExitosos++;
                            reporteFinalKoha.Add(new EstudianteKohaNotificacionDto
                            {
                                BannerId = est.BannerId!,
                                Nombre = $"{est.Nombre} {est.Apellido}".Trim() ?? "N/D",
                                CorreoInstitucional = est.Correo ?? "N/D",
                                Estado = est.Estado ?? "N/D",
                                EsExito = true,
                                FechaProceso = DateTime.Now
                            });

                            var nuevoLog = new koha_log
                            {
                                Codigo_Estudiante = est.BannerId!,
                                Correo_estudiante = est.Correo,
                                banner_JSON = JsonSerializer.Serialize(est),
                                Fecha_Creacion = DateTime.Now,
                                Usuario_Crea = usuarioCreador,
                                Host = host
                            };

                            await _context.koha_logs.AddAsync(nuevoLog);
                        }
                        else
                        {
                            kohaPostFallidos++;
                            reporteFinalKoha.Add(new EstudianteKohaNotificacionDto
                            {
                                BannerId = est.BannerId!,
                                Nombre = $"{ est.Nombre}{est.Apellido}".Trim() ?? "N/D",
                                CorreoInstitucional = est.Correo ?? "N/D",
                                Estado = est.Estado ?? "N/D",
                                EsExito = false,
                                FechaProceso = DateTime.Now
                            });
                        }
                    }

                    if (kohaPostExitosos > 0)
                    {
                        await _context.SaveChangesAsync();
                    }
                }

                await EnviarNotificacionSincronizacionAsync(reporteFinalKoha);

                return new
                {
                    fechaAnalisis = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    totalEstudiantesBanner = students.Count,
                    totalUsuariosExistentesKoha = usuariosKoha.Count,
                    cantidadYaExistentes = estudiantesYaExistentes.Count,
                    cantidadNuevosAInsertar = estudiantesNuevosAInsertar.Count,
                    kohaPostExitosos,
                    kohaPostFallidos,
                    estudiantesNuevos = estudiantesNuevosAInsertar
                };
            }
            catch (Exception ex)
            {
                // ==========================================================
                // CAPTURA DE ERROR GLOBAL: SE DISPARA AL CORREO DE ERRORES
                // ==========================================================
                try
                {
                    await EnviarNotificacionErrorGlobalAsync(ex, usuarioCreador);
                }
                catch (Exception mailEx)
                {
                    Console.WriteLine($"Error crítico secundario mandando correo de alerta: {mailEx.Message}");
                }

                throw;
            }
        }

        // MÉTODO A: Envío de Resumen Ordinario de Sincronización
        private async Task EnviarNotificacionSincronizacionAsync(List<EstudianteKohaNotificacionDto> estudiantesProcesados)
        {
            string rutaPlantilla = Path.Combine(AppContext.BaseDirectory, "Templates", "EmailBanner.html");
            if (!File.Exists(rutaPlantilla)) return;

            string contenidoHtml = await File.ReadAllTextAsync(rutaPlantilla);
            StringBuilder filasHtml = new StringBuilder();
            string fechaEjecucion = DateTime.Now.ToString("yyyy-MM-dd");

            if (estudiantesProcesados.Any())
            {
                foreach (var est in estudiantesProcesados)
                {
                    // Determinar colores según el estado
                    string colorEstado = est.EsExito ? "#048047" : "#DC3545";

                    filasHtml.Append("<tr style='border-collapse:collapse'>");
                    filasHtml.Append($"<td align='left' style='padding:12px 15px; border-bottom:1px solid #dee2e6; font-size:13px; color:#495057;'>{est.BannerId}</td>");
                    filasHtml.Append($"<td align='left' style='padding:12px 15px; border-bottom:1px solid #dee2e6; font-size:13px; color:#495057;'>{est.Nombre}</td>");
                    filasHtml.Append($"<td align='left' style='padding:12px 15px; border-bottom:1px solid #dee2e6; font-size:13px; color:#495057;'>{est.CorreoInstitucional}</td>");
                    filasHtml.Append($"<td align='left' style='padding:12px 15px; border-bottom:1px solid #dee2e6; font-size:13px; color:{colorEstado}; font-weight:bold;'>{est.Estado}</td>");
                    filasHtml.Append($"<td align='left' style='padding:12px 15px; border-bottom:1px solid #dee2e6; font-size:13px; color:#495057;'>{fechaEjecucion}</td>");
                    filasHtml.Append("</tr>");
                }
            }
            else
            {
                filasHtml.Append("<tr style='border-collapse:collapse'>");
                filasHtml.Append("<td colspan='5' align='center' style='padding:20px; border:1px solid #dee2e6; font-size:14px; color:#6c757d; font-style:italic;'>No se encontraron alumnos nuevos para sincronizar.</td>");
                filasHtml.Append("</tr>");
            }

            string tituloReporte = estudiantesProcesados.Any() ? "Perfiles En Koha" : "Perfiles En Koha";
            string descripcionTexto = estudiantesProcesados.Count > 0 ? "A continuación, se muestra el detalle de los estudiantes a los que se les creó perfil en la plataforma de Koha desde la información de Banner." : "No hubo ningún perfil para crear en la plataforma de Koha.";


            contenidoHtml = contenidoHtml.Replace("{{TITULO}}", tituloReporte);
            contenidoHtml = contenidoHtml.Replace("{{CANTIDAD}}", estudiantesProcesados.Count.ToString());
            contenidoHtml = contenidoHtml.Replace("{{DESCRIPCION}}", descripcionTexto);
            contenidoHtml = contenidoHtml.Replace("{{CUENTAS}}", filasHtml.ToString());
            contenidoHtml = contenidoHtml.Replace("API RRHH", "Integración Banner - Koha (Biblioteca)");

            string cadenaCorreos = _configuration["ZamoMails:DestinatarioResumen"] ?? "desarrollonotificaciones@zamorano.edu";
            string destinatariosFormateados = cadenaCorreos.Replace(';', ',');

            var payload = new MailsModel
            {
                To = destinatariosFormateados,
                Subject = tituloReporte,
                Body = contenidoHtml
            };

            await _mailsService.SendEmailAsync("/mails/v1/Send", payload);
        }

        // MÉTODO B: Envío Exclusivo de Fallos Críticos (DestinatarioResumenError)
        private async Task EnviarNotificacionErrorGlobalAsync(Exception excepcion, string usuario)
        {
            string fechaError = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            // Construcción de un cuerpo HTML limpio e informativo para reportar el problema técnico
            StringBuilder htmlBuilder = new StringBuilder();
            htmlBuilder.Append("<div style='font-family:Arial, sans-serif; padding:20px; border:1px solid #dc3545; border-radius:5px; background-color:#fffdfd;'>");
            htmlBuilder.Append("<h2 style='color:#dc3545; margin-top:0;'>⚠️ Alerta Crítica: Fallo en Sincronización Banner-Koha</h2>");
            htmlBuilder.Append($"<p><strong>Fecha del Evento:</strong> {fechaError}</p>");
            htmlBuilder.Append($"<p><strong>Usuario Ejecutor:</strong> {usuario}</p>");
            htmlBuilder.Append("<hr style='border:0; border-top:1px solid #dee2e6;'/>");
            htmlBuilder.Append("<p style='font-size:16px; font-weight:bold; color:#333;'>Detalle del Mensaje de Error:</p>");
            htmlBuilder.Append($"<div style='background-color:#f8f9fa; padding:15px; border-left:4px solid #dc3545; font-family:Courier New, monospace; font-size:13px; white-space:pre-wrap;'>{excepcion.Message}</div>");
            htmlBuilder.Append("<p style='font-size:14px; font-weight:bold; color:#333; margin-top:15px;'>Seguimiento de la Pila (Stack Trace):</p>");
            htmlBuilder.Append($"<div style='background-color:#f8f9fa; padding:15px; font-family:Courier New, monospace; font-size:11px; color:#6c757d; max-height:250px; overflow-y:auto; white-space:pre-wrap;'>{excepcion.StackTrace}</div>");
            htmlBuilder.Append("<p style='margin-top:20px; font-size:12px; color:#868e96; font-style:italic;'>Este correo fue despachado automáticamente por el Middleware de Integración de Datos de la Biblioteca.</p>");
            htmlBuilder.Append("</div>");

            // Extrae de forma precisa la lista asignada para la administración de errores
            string cadenaCorreosError = _configuration["ZamoMails:DestinatarioResumenError"] ?? "jmairena@zamorano.edu";
            string destinatariosErrorFormateados = cadenaCorreosError.Replace(';', ',');

            var payloadError = new MailsModel
            {
                To = destinatariosErrorFormateados,
                Subject = "🚨 ERROR CRÍTICO: Sincronización Automática Banner - Koha",
                Body = htmlBuilder.ToString()
            };

            await _mailsService.SendEmailAsync("/mails/v1/Send", payloadError);
        }
    }
}
