using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using ApiEnvioMasivo.Data;
using ApiEnvioMasivo.Services;
using Hangfire;
using ApiEnvioMasivo.Filters;



namespace ApiEnvioMasivo
{
    public class Startup
    {
        public IConfiguration Configuration { get; }


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;


        }

        public void ConfigureServices(IServiceCollection services)
        {
           
            services.AddHttpClient();

            var modoCorreo = Configuration["ModoCorreo"];

            if (modoCorreo == "infobip")
            {
                services.AddScoped<IEmailService, InfobipEmailService>();
            }
            else
            {
                services.AddScoped<IEmailService, MockEmailService>();
            }

            services.AddControllers();

            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<FlujoRunnerService>();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API Envío Masivo",
                    Version = "v1"
                });
            });

            // 1️⃣ Hangfire
            services.AddHangfire(cfg =>
                cfg.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));
            services.AddHangfireServer();
            // Esto es una prueba para disparar un deploy automático

            services.AddScoped<LogService>();

            services.AddScoped<IFlujoService, FlujoRunnerService>();

            services.AddScoped<MockEmailService>();


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseDeveloperExceptionPage(); // habilita trazas detalladas

   

            app.UseHttpsRedirection(); // Redirección a HTTPS

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Envío Masivo V1");
            });

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                // 👇 Mapeo del dashboard Hangfire para que funcione en Render
                endpoints.MapHangfireDashboard("/hangfire", new DashboardOptions
                {
                    Authorization = new[] { new AllowAllDashboardAuthorizationFilter() }
                });
            });

            //using (var scope = app.ApplicationServices.CreateScope())
            //{
            //    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            //    FlujoSeeder.CargarFlujoDeEjemplo(db);
            //}

            // 🔁 Ejecutar flujos automáticamente cada 5 minutos
            //RecurringJob.AddOrUpdate<IFlujoService>(
            //    "ejecutar-flujos-recurrente",
            //    x => x.EjecutarTodosLosFlujosAsync(),
            //    "*/5 * * * *"
            //);
        }

    }
}
