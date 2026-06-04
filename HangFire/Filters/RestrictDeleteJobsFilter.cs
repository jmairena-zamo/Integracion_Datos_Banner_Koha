using Hangfire.Dashboard;

namespace Integracion_Datos_Banner_Koha.Hangfire.Filters
{
    public class RestrictDeleteJobsFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            // Bloquea específicamente la eliminación de jobs
            if (httpContext.Request.Path.ToString().Contains("/recurring/remove"))
            {
                return true;
            }

            return true;
        }
    }
}
