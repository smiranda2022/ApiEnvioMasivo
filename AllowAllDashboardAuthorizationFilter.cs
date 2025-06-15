using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace ApiEnvioMasivo.Filters
{
    public class AllowAllDashboardAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            return true;
        }
    }
}