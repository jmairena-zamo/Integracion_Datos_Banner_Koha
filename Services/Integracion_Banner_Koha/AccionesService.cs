using Hangfire;
using Integracion_Datos_Banner_Koha.Context;
using Integracion_Datos_Banner_Koha.Models.DB.Biblioteca;
using Integracion_Datos_Banner_Koha.Models.DTOs;
using Integracion_Datos_Banner_Koha.Services.Integracion_Banner_Koha.Interfaces;
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

        public AccionesService(IBannerSyncService bannerService, IKohaSyncService kohaService, BibliotecaContext context)
        {
            _bannerService = bannerService;
            _kohaService = kohaService;
            _context = context;
        }

        public async Task<object> AnalizarYCruzarDatosAsync(string usuarioCreador, string host)
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

                        var nuevoLog = new koha_log
                        {
                            // El ID no se mapea, la base de datos lo autoincrementa
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
                        }
                    }
                if (kohaPostExitosos > 0)
                {
                    await _context.SaveChangesAsync();
                }
            }

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
    }
}
