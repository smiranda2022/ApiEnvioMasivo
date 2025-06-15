using Hangfire.Dashboard;

public class AllowAllDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        // ⚠️ En producción real usá autenticación.
        return true;   // Permite el acceso a cualquiera.
    }
}


