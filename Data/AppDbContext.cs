using Microsoft.EntityFrameworkCore;
using ApiEnvioMasivo.Models;

namespace ApiEnvioMasivo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Destinatario> Destinatarios { get; set; }
        public DbSet<Flujo> Flujos { get; set; }
        public DbSet<FlujoPaso> FlujoPasos { get; set; }
        public DbSet<FlujoHistorial> FlujoHistoriales { get; set; }

        public DbSet<CorreoEnviado> CorreosEnviados { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Ya tenés esto para SuscripcionRequest
            modelBuilder.Entity<SuscripcionRequest>().HasNoKey();

            modelBuilder.Entity<CorreoEnviado>()
           .HasKey(e => e.Id); // ✅ clave primaria


            // 🔧 Configuración para evitar ambigüedad
            modelBuilder.Entity<CorreoEnviado>()
                .HasOne(e => e.FlujoPaso)
                .WithMany()
                .HasForeignKey(e => e.FlujoPasoId);

            // También podrías configurar la relación con Destinatario si da conflicto
            modelBuilder.Entity<CorreoEnviado>()
                .HasOne(e => e.Destinatario)
                .WithMany()
                .HasForeignKey(e => e.DestinatarioId);
        }

    }
}