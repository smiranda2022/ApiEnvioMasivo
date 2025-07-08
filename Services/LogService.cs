using ApiEnvioMasivo.Data;
using ApiEnvioMasivo.Models;
using System.Threading.Tasks;
using System;

namespace ApiEnvioMasivo.Services
{
    public class LogService
    {
        private readonly AppDbContext _db;

        public LogService(AppDbContext db)
        {
            _db = db;
        }

        public async Task GuardarLogAsync(string tipo, string contenido)
        {
            var log = new Log
            {
                Fecha = DateTime.Now,
                Tipo = tipo,
                Contenido = contenido
            };

            _db.Logs.Add(log);
            await _db.SaveChangesAsync();
        }
    }

}
