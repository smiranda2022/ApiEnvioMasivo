
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
            await Task.Delay(500); // Simula envío

            var fakeResponse = new
            {
                Simulado = true,
                Destinatario = to,
                Asunto = subject,
                Estado = "PENDING_ACCEPTED",
                Fecha = DateTime.UtcNow
            };

            await _log.GuardarLogAsync("MOCK_EMAIL", JsonConvert.SerializeObject(fakeResponse));

            // Buscar destinatario real
            var destinatario = await _db.Destinatarios.FirstOrDefaultAsync(d => d.Email == to);
            var paso = await _db.FlujoPasos.FirstOrDefaultAsync(); // Esto podrías mejorar

            if (destinatario != null && paso != null)
            {
                var correo = new CorreoEnviado
                {
                    DestinatarioId = destinatario.Id,
                    FlujoPasoId = paso.Id,
                    FechaEnvio = DateTime.UtcNow,
                    Abierto = false
                };

                _db.CorreosEnviados.Add(correo);
                await _db.SaveChangesAsync();
            }

            return "Simulado correctamente";
        }


    }

}
