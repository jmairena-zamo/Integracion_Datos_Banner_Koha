using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Integracion_Datos_Banner_Koha.Models
{
    public class SyncAuditModel
    {
        public string UsuarioEjecutor { get; set; }
        public DateTime FechaEjecucion { get; set; }
        public double TiempoDemoraSegundos { get; set; }

        // Listas para guardar qué estudiantes sufrieron cambios
        public List<string> EstudiantesCreados { get; set; } = new List<string>();
        public List<string> EstudiantesActualizados { get; set; } = new List<string>();

        private readonly Stopwatch _cronometro;

        public SyncAuditModel(string usuario)
        {
            UsuarioEjecutor = usuario;
            FechaEjecucion = DateTime.Now;
            _cronometro = Stopwatch.StartNew();
        }

        public void FinalizarProceso()
        {
            _cronometro.Stop();
            TiempoDemoraSegundos = Math.Round(_cronometro.Elapsed.TotalSeconds, 2);
        }
    }
}
