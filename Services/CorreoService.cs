using ApiEnvioMasivo.Data;
using ApiEnvioMasivo.Models;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;

namespace ApiEnvioMasivo.Services
{
    public class CorreoService
    {
        private readonly AppDbContext _db;
        private readonly LogService _log;

        public CorreoService(AppDbContext db, LogService log)
        {
            _db = db;
            _log = log;
        }

        public async Task<object> EnviarCorreo(string email, string nombre, int destinatarioId, int pasoId, string asunto, string html)
        {
            var enviado = new CorreoEnviado
            {
                DestinatarioId = destinatarioId,
                FlujoPasoId = pasoId,
                FechaEnvio = DateTime.UtcNow,
                Abierto = false
            };
            _db.CorreosEnviados.Add(enviado);
            await _db.SaveChangesAsync();

            // Acá podés agregar el llamado real al email service (mock o infobip)
            // Por ahora, algo así:
            var resultado = new
            {
                Email = email,
                Success = true,
                Status = "Simulado",
                Description = "Enviado con éxito"
            };

            await _log.GuardarLogAsync("ENVIO", JsonConvert.SerializeObject(resultado));
            return resultado;
        }
    }

}
