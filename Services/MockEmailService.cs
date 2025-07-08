
using System;
using System.Threading.Tasks;
using ApiEnvioMasivo.Data;
using ApiEnvioMasivo.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;


namespace ApiEnvioMasivo.Services
{
   
    public interface IEmailService
    {
        Task<string> EnviarCorreoAsync(string to, string subject, string html);
    }

    public class MockEmailService : IEmailService
    {
        private readonly LogService _log;
        private readonly AppDbContext _db;

        public MockEmailService(LogService log, AppDbContext db)
        {
            _log = log;
            _db = db;
        }

        public async Task<string> EnviarCorreoAsync(string to, string subject, string html)
        {
            // Simula un retardo como si se estuviera enviando
            await Task.Delay(500);

            // Construye una respuesta simulada
            var fakeResponse = new
            {
                Simulado = true,
                Destinatario = to,
                Asunto = subject,
                Estado = "PENDING_ACCEPTED",
                Fecha = DateTime.UtcNow
            };

            // Guarda el log en la base
            await _log.GuardarLogAsync("MOCK_EMAIL", JsonConvert.SerializeObject(fakeResponse));

            // Guarda también en la tabla CorreosEnviados
            var correo = new CorreoEnviado
            {
                DestinatarioId = 7,  // solo para pruba
                FlujoPasoId = 1,
                FechaEnvio = DateTime.UtcNow,
                Abierto = false
            };

            _db.CorreosEnviados.Add(correo);
            await _db.SaveChangesAsync();

            return "Simulado correctamente";
        }
    }

}
