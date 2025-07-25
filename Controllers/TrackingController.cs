﻿using Microsoft.AspNetCore.Mvc;
using ApiEnvioMasivo.Data;
using System.Threading.Tasks;
using System.IO;
using System;
using Microsoft.EntityFrameworkCore;

namespace ApiEnvioMasivo.Controllers
{
    [ApiController]
    [Route("api/tracking")]
    public class TrackingController : ControllerBase
    {
        private readonly AppDbContext _db;

        public TrackingController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet("open")]

        public async Task<IActionResult> RegistrarApertura([FromQuery] int correoId)
        {
            var registro = await _db.CorreosEnviados
                .FirstOrDefaultAsync(x => x.Id == correoId);

            if (registro != null && !registro.Abierto)
            {
                registro.Abierto = true;
                registro.FechaApertura = DateTime.UtcNow;

                // Actualizar FlujoHistorial solo por DestinatarioId
                var historial = await _db.FlujoHistoriales.FirstOrDefaultAsync(h =>
                    h.DestinatarioId == registro.DestinatarioId
                );

                if (historial != null)
                {
                    historial.Abierto = true;
                }

                await _db.SaveChangesAsync();
            }

            // Pixel 1x1 transparente
            var pixel = new byte[]
            {
        71,73,70,56,57,97,1,0,
        1,0,128,0,0,255,255,255,
        0,0,0,33,249,4,1,0,0,
        0,0,44,0,0,0,0,1,0,
        1,0,2,2,76,1,0,59
            };

            return File(pixel, "image/gif");
        }



    }
}
