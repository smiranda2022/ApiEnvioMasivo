using Hangfire.Dashboard;

namespace ApiEnvioMasivo.Filters   // ← este namespace debe existir
{
    public class AllowAllDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}
