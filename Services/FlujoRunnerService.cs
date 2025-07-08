using ApiEnvioMasivo.Data;
using ApiEnvioMasivo.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using System.Text.Json;
using Microsoft.Extensions.Configuration;


namespace ApiEnvioMasivo.Services
{
    public class FlujoRunnerService : IFlujoService
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly MockEmailService _mockEmailService;

        public FlujoRunnerService(AppDbContext db, IConfiguration configuration, MockEmailService mockEmailService)
        {
            _db = db;
            _configuration = configuration;
            _mockEmailService = mockEmailService;
        }

        //public async Task EjecutarFlujosAsync()
        //{
        //    var ahora = DateTime.UtcNow;

        //    var destinatarios = await _db.Destinatarios
        //        .Include(d => d.Flujo)
        //        .ThenInclude(f => f.Pasos)
        //        .ToListAsync();

        //    foreach (var d in destinatarios)
        //    {
        //        if (d.Flujo == null || !d.Flujo.Activo)
        //            continue;

        //        foreach (var paso in d.Flujo.Pasos
        //            .Where(p => p.Activo)
        //            .OrderBy(p => p.Orden))
        //        {
        //            // Validar si ya fue enviado
        //            bool yaEnviado = await _db.CorreosEnviados
        //                .AnyAsync(c => c.DestinatarioId == d.Id && c.FlujoPasoId == paso.Id);

        //            if (yaEnviado)
        //                continue;

        //            // Verificar si ya pasó el tiempo de espera desde la fecha de inicio
        //            if (d.FechaInicioFlujo + paso.Espera > ahora)
        //                continue;

        //            // Validar condición (ejemplo básico)
        //            if (paso.CondicionTipo == "no_abierto")
        //            {
        //                var abierto = await _db.CorreosEnviados
        //                    .Where(c => c.DestinatarioId == d.Id && c.FlujoPasoId == paso.Id - 1)
        //                    .Select(c => c.Abierto)
        //                    .FirstOrDefaultAsync();

        //                if (abierto) continue;
        //            }

        //            // TODO: Llamar a tu método de envío aquí
        //            await EnviarCorreoAsync(d.Email, d.Nombre, paso.Asunto, paso.HtmlContenido);



        //            var enviado = new CorreoEnviado
        //            {
        //                DestinatarioId = d.Id,
        //                FlujoPasoId = paso.Id,
        //                FechaEnvio = ahora,
        //                Abierto = false
        //            };

        //            _db.CorreosEnviados.Add(enviado);
        //            await _db.SaveChangesAsync(); // Acá se genera el ID

        //            // Tracking Pixel
        //            var baseUrl = _configuration["TrackingBaseUrl"];
        //            string pixel = $"<img src='{baseUrl}/api/tracking/open?correoId={enviado.Id}' width='1' height='1' style='display:none;' />";
        //            var htmlFinal = paso.HtmlContenido.Replace("{{nombre}}", d.Nombre) + pixel;

        //            // Ahora usar htmlFinal para enviarlo
        //            //await EnviarCorreo(d.Email, paso.Asunto, htmlFinal);
        //            var resultado = await EnviarCorreoHtml(d.Email, paso.Asunto, htmlFinal);
        //            ///var resultado = await _correoService.EnviarCorreoHtml(d.Email, paso.Asunto, htmlFinal);

        //        }
        //    }
        //}

        public async Task EjecutarFlujosAsync()
        {
            var ahora = DateTime.UtcNow;

            var destinatarios = await _db.Destinatarios
                .Include(d => d.Flujo)
                .ThenInclude(f => f.Pasos)
                .ToListAsync();

            foreach (var d in destinatarios)
            {
                if (d.Flujo == null || !d.Flujo.Activo)
                    continue;

                var pasos = d.Flujo.Pasos
                    .Where(p => p.Activo)
                    .OrderBy(p => p.Orden)
                    .ToList();

                for (int i = 0; i < pasos.Count; i++)
                {
                    var paso = pasos[i];

                    bool yaEnviado = await _db.CorreosEnviados
                        .AnyAsync(c => c.DestinatarioId == d.Id && c.FlujoPasoId == paso.Id);
                    if (yaEnviado)
                        continue;

                    if (i > 0)
                    {
                        var pasoAnterior = pasos[i - 1];
                        var envioAnterior = await _db.CorreosEnviados
                            .Where(c => c.DestinatarioId == d.Id && c.FlujoPasoId == pasoAnterior.Id)
                            .FirstOrDefaultAsync();

                        if (envioAnterior == null)
                            break;

                        if (envioAnterior.FechaEnvio + paso.Espera > ahora)
                            break;

                        if (paso.CondicionTipo == "no_abierto" && envioAnterior.Abierto == true)
                            break;
                    }
                    else
                    {
                        if (d.FechaInicioFlujo + paso.Espera > ahora)
                            break;
                    }

                    await EnviarCorreoAsync(d.Email, d.Nombre, paso.Asunto, paso.HtmlContenido);

                    var enviado = new CorreoEnviado
                    {
                        DestinatarioId = d.Id,
                        FlujoPasoId = paso.Id,
                        FechaEnvio = ahora,
                        Abierto = false
                    };

                    _db.CorreosEnviados.Add(enviado);
                    await _db.SaveChangesAsync();

                    var baseUrl = _configuration["TrackingBaseUrl"];
                    string pixel = $"<img src='{baseUrl}/api/tracking/open?correoId={enviado.Id}' width='1' height='1' style='display:none;' />";
                    var htmlFinal = paso.HtmlContenido.Replace("{{nombre}}", d.Nombre) + pixel;

                    var resultado = await EnviarCorreoHtml(d.Email, paso.Asunto, htmlFinal);

                    break; // Detener después de ejecutar un paso
                }
            }
        }
        private async Task EnviarCorreoAsync(string email, string nombre, string asunto, string html)
        {
            // Aquí podrías reutilizar tu lógica de envío con Infobip
            Console.WriteLine($"📨 Enviando a {nombre} <{email}>: {asunto}");
            await Task.Delay(100); // Simula envío
        }
        private async Task<object> EnviarCorreoHtml(string email, string asunto, string htmlContenido)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new { Email = email, Success = false, Error = "Email está vacío." };
            }

            try
            {
                var client = new RestClient("https://kq8928.api.infobip.com");
                var request = new RestRequest("/email/3/send", Method.Post);

                request.AddHeader("Authorization", "App 4d0c4548a64868ddc26011c2dc9b3ff3-8d1301f0-4cc8-4d5f-b975-d267b2a2513c");
                request.AddHeader("Content-Type", "multipart/form-data");
                request.AddHeader("Accept", "application/json");
                request.AlwaysMultipartFormData = true;

                request.AddParameter("from", "MiFelicidadMascotas <info@mifelicidadmascotas.com>");
                request.AddParameter("subject", asunto);
                request.AddParameter("to", $"{{\"to\":\"{email}\"}}"); // sin placeholders

                request.AddParameter("html", htmlContenido);

                var response = await client.ExecuteAsync(request);

                if (!response.IsSuccessful)
                {
                    return new { Email = email, Success = false, StatusCode = response.StatusCode, Error = response.Content };
                }

                var json = JsonDocument.Parse(response.Content);
                var message = json.RootElement.GetProperty("messages")[0];
                var status = message.GetProperty("status");

                return new
                {
                    Email = email,
                    Success = true,
                    Status = status.GetProperty("name").GetString(),
                    Description = status.GetProperty("description").GetString()
                };
            }
            catch (Exception ex)
            {
                return new { Email = email, Success = false, Error = "Excepción: " + ex.Message };
            }
        }
        public async Task<string> EjecutarFlujoAsync(int flujoId)
        {
            try
            {
                var flujo = await _db.Flujos
                    .Include(f => f.Pasos)
                    .FirstOrDefaultAsync(f => f.Id == flujoId);

                if (flujo == null || flujo.Pasos == null || !flujo.Pasos.Any())
                    return $"Flujo {flujoId} no encontrado o sin pasos.";

                var destinatarios = await _db.Destinatarios.ToListAsync();
                if (!destinatarios.Any())
                    return "No hay destinatarios para ejecutar el flujo.";

                foreach (var paso in flujo.Pasos.OrderBy(p => p.Orden))
                {
                    foreach (var destinatario in destinatarios)
                    {
                        // Enviar correo con HTML fijo (sin usar CuerpoHtml)
                        await _mockEmailService.EnviarCorreoAsync(
                            destinatario.Email,
                            paso.Asunto,
                            "<p>Correo de prueba: este es el paso del flujo.</p>");

                        // Registrar en FlujoHistoriales
                        var historial = new FlujoHistorial
                        {
                            DestinatarioId = destinatario.Id,
                            FlujoId = flujo.Id,
                            PasoId = paso.Id,
                            FechaEnvio = DateTime.UtcNow,
                            Abierto = false,
                            HizoClic = false
                        };

                        _db.FlujoHistoriales.Add(historial);
                    }
                }

                await _db.SaveChangesAsync();
                return $"Flujo {flujoId} ejecutado correctamente para {destinatarios.Count} destinatarios.";
            }
            catch (Exception ex)
            {
                return $"Error al ejecutar flujo: {ex.InnerException?.Message ?? ex.Message}";
            }
        }


        //public async Task<string> EjecutarFlujoAsync(int flujoId)
        //{
        //    var flujo = await _db.Flujos
        //        .Include(f => f.Pasos)
        //        .FirstOrDefaultAsync(f => f.Id == flujoId);

        //    if (flujo == null || flujo.Pasos == null || !flujo.Pasos.Any())
        //        return $"Flujo {flujoId} no encontrado o sin pasos.";

        //    var destinatarios = await _db.Destinatarios.ToListAsync();
        //    if (!destinatarios.Any())
        //        return "No hay destinatarios para ejecutar el flujo.";

        //    foreach (var paso in flujo.Pasos.OrderBy(p => p.Orden))
        //    {
        //        foreach (var destinatario in destinatarios)
        //        {
        //            // Acá iría el envío real o mock:
        //            //await _mockEmailService.EnviarCorreoAsync(destinatario.Email, paso.Asunto, paso.CuerpoHtml);
        //            await _mockEmailService.EnviarCorreoAsync(destinatario.Email, paso.Asunto, "<p>Este es un correo de prueba</p>");


        //            // Registrar en FlujoHistoriales:
        //            var historial = new FlujoHistorial
        //            {
        //                DestinatarioId = destinatario.Id,
        //                FlujoId = flujo.Id,
        //                PasoId = paso.Id,
        //                FechaEnvio = DateTime.UtcNow,
        //                Abierto = false,
        //                HizoClic = false
        //            };

        //            _db.FlujoHistoriales.Add(historial);

        //        }
        //    }

        //    await _db.SaveChangesAsync();
        //    return $"Flujo {flujoId} ejecutado correctamente para {destinatarios.Count} destinatarios.";
        //}


        public async Task EjecutarTodosLosFlujosAsync()
        {
            // Lógica para ejecutar todos los flujos
            await Task.CompletedTask;
        }




    }
}
