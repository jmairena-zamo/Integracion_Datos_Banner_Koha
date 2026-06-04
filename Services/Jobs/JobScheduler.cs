using Hangfire;

namespace Integracion_Datos_Banner_Koha.Services.Jobs
{
    public class JobScheduler
    {
        private readonly string queues;
        private readonly bool isJobEnabled;
        private readonly RecurringJobOptions recurringJobOptions;

        public JobScheduler(IConfiguration configuration)
        {
            queues = configuration.GetValue<string>("Queues")?.ToLower() ?? "default";
            isJobEnabled = configuration.GetValue<bool>("Jobs:EnableRecurringJobs");

            recurringJobOptions = new RecurringJobOptions()
            {
                TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time")
            };
        }

        public void RegisterJobs()
        {
            AccionesServiceJob.RegisterJobs(queues, recurringJobOptions, isJobEnabled);
        }
    }
}
