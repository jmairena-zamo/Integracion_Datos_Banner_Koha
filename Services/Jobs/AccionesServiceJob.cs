using Hangfire;
using Integracion_Datos_Banner_Koha.Services.Integracion_Banner_Koha.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Integracion_Datos_Banner_Koha.Services.Jobs
{
    public class AccionesServiceJob
    {
        private static void CreateAccionesServiceJob(string queues, RecurringJobOptions recurringJobOptions, bool isJobEnabled)
        {
            if (!isJobEnabled) return;

            string recurringJobId = "INTEGRACION_BANNER_KOHA";
            RecurringJob.RemoveIfExists(recurringJobId);

            string usuario = "Api_Koha";
            string host = Environment.MachineName;

            RecurringJob.AddOrUpdate<IAccionesService>(
                recurringJobId,
                queues,
                x => x.AnalizarYCruzarDatosAsync(usuario, host),
                "0 2 * * *",
                recurringJobOptions
            );
        }

        public static void RegisterJobs(string queues, RecurringJobOptions recurringJobOptions, bool isJobEnabled)
        {
            var horaActual = DateTime.Now.TimeOfDay;

            CreateAccionesServiceJob(queues, recurringJobOptions, isJobEnabled);
        }
    }
}
