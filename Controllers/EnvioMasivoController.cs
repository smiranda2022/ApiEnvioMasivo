using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Collections.Generic;
using ApiEnvioMasivo.Models;
using System.Threading.Tasks; // 👈 AGREGA ESTA LÍNEA
using OfficeOpenXml; // ⬅️ Asegurate de tener esta línea arriba (requiere EPPlus)
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text.Json;
using System.Linq;
using System;
using ApiEnvioMasivo.Data;
using Microsoft.EntityFrameworkCore;
using ApiEnvioMasivo.Services;

namespace ApiEnvioMasivo.Controllers
{
    [ApiController]
    [Route("api/suscripcion")]
    public class EnvioMasivoController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> EnviarCorreoIndividual([FromBody] SuscripcionRequest request)
        {
            var resultado = await EnviarCorreo(request.Email, request.Nombre);
           
            return Ok(resultado);
        }

        [HttpPost("masivo")]
        public async Task<IActionResult> EnviarCorreosMasivos([FromBody] SuscripcionMasivaRequestConSegmentacion request)
        {
            var destinatariosFiltrados = request.Destinatarios.Where(d =>
                (!request.Segmentacion.Edad.HasValue || d.Edad == request.Segmentacion.Edad.Value) &&
                (string.IsNullOrEmpty(request.Segmentacion.Genero) || d.Genero.Equals(request.Segmentacion.Genero, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(request.Segmentacion.Ubicacion) || d.Ubicacion.Equals(request.Segmentacion.Ubicacion, StringComparison.OrdinalIgnoreCase))
            ).ToList();

            var resultados = new List<object>();
            foreach (var d in destinatariosFiltrados)
            {
                var resultado = await EnviarCorreo(d.Email, d.Nombre);
                resultados.Add(resultado);
            }
            return Ok(new
            {
                TotalEnviados = resultados.Count,
                Resultados = resultados
            });
        }

        [HttpPost("masivo-db")]
        public async Task<IActionResult> EnviarDesdeBaseDeDatos([FromBody] SegmentacionModel filtros, [FromServices] AppDbContext db)
        {
            var query = db.Destinatarios.AsQueryable();

            if (filtros.Edad.HasValue)
                query = query.Where(d => d.Edad == filtros.Edad.Value);

            if (!string.IsNullOrEmpty(filtros.Genero))
                query = query.Where(d => d.Genero.ToLower() == filtros.Genero.ToLower());

            if (!string.IsNullOrEmpty(filtros.Ubicacion))
                query = query.Where(d => d.Ubicacion.ToLower() == filtros.Ubicacion.ToLower());
            var destinatarios = await query.ToListAsync(); // ✅ ejecuta el query filtrado



            var resultados = new List<object>();
            foreach (var d in destinatarios)
            {
                if (string.IsNullOrWhiteSpace(d.Email) || string.IsNullOrWhiteSpace(d.Nombre))
                    continue;

               var resultado = await EnviarCorreo(d.Email, d.Nombre);
                //var resultado =  await EnviarCorreoHtml(d.Email, Asunto, Html);


                resultados.Add(resultado);
            }

            return Ok(new
            {
                TotalEnviados = resultados.Count,
                Resultados = resultados
            });
        }

        [HttpGet("destinatarios")]
        public async Task<IActionResult> ObtenerDestinatarios([FromServices] AppDbContext db)
        {
            var lista = await db.Destinatarios.ToListAsync();

            return Ok(new
            {
                Total = lista.Count,
                Datos = lista
            });
        }

        [HttpGet("probar-flujo")]
        public async Task<IActionResult> EjecutarFlujoAhora([FromServices] FlujoRunnerService runner)
        {
            await runner.EjecutarFlujosAsync();
            return Ok("✅ Flujo ejecutado manualmente");
        }

        [HttpGet("historial-flujos")]
        public async Task<IActionResult> VerHistorial([FromServices] AppDbContext db)
        {
            var historial = await db.CorreosEnviados
                .Include(c => c.Destinatario)
                .Include(c => c.FlujoPaso)
                .Select(c => new
                {
                    Email = c.Destinatario.Email,
                    Nombre = c.Destinatario.Nombre,
                    Paso = c.FlujoPaso.Orden,
                    Asunto = c.FlujoPaso.Asunto,
                    Fecha = c.FechaEnvio,
                    Abierto = c.Abierto
                })
                .OrderByDescending(c => c.Fecha)
                .ToListAsync();

            return Ok(historial);
        }


        [HttpPost("agregar")]
        public async Task<IActionResult> AgregarDestinatario([FromBody] Destinatario nuevo, [FromServices] AppDbContext db)
        {
            if (string.IsNullOrWhiteSpace(nuevo.Email) || string.IsNullOrWhiteSpace(nuevo.Nombre))
                return BadRequest("Email y Nombre son obligatorios.");

            await db.Destinatarios.AddAsync(nuevo);
            await db.SaveChangesAsync();

            return Ok(new
            {
                Mensaje = "Destinatario agregado correctamente.",
                nuevo.Id,
                nuevo.Email,
                nuevo.Nombre
            });
        }

        [HttpPost("masivo-excel")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> EnviarCorreosDesdeExcel([FromForm] IFormFile archivo)
        {
            ExcelPackage.License.SetNonCommercialPersonal("Santiago Miranda");

            if (archivo == null || archivo.Length == 0)
                return BadRequest("Debe subir un archivo Excel válido.");

            var destinatarios = new List<SuscripcionExtendidaRequest>();

            using (var stream = new MemoryStream())
            {
                await archivo.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var hoja = package.Workbook.Worksheets[0];
                    int filas = hoja.Dimension.Rows;

                    for (int fila = 2; fila <= filas; fila++)
                    {
                        var email = hoja.Cells[fila, 1].Text;
                        var nombre = hoja.Cells[fila, 2].Text;
                        var edad = int.TryParse(hoja.Cells[fila, 3].Text, out var e) ? e : 0;
                        var genero = hoja.Cells[fila, 4].Text;
                        var ubicacion = hoja.Cells[fila, 5].Text;

                        if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(nombre))
                        {
                            destinatarios.Add(new SuscripcionExtendidaRequest
                            {
                                Email = email,
                                Nombre = nombre,
                                Edad = edad,
                                Genero = genero,
                                Ubicacion = ubicacion
                            });
                        }
                    }
                }
            }

            // 👇 Segmentación simulada fija (se puede recibir como parámetro si lo deseás)
            var filtro = new SegmentacionModel
            {
                Edad = 25,
                Genero = "F",
                Ubicacion = "Buenos Aires"
            };

            var destinatariosFiltrados = destinatarios.Where(d =>
                (!filtro.Edad.HasValue || d.Edad == filtro.Edad.Value) &&
                (string.IsNullOrEmpty(filtro.Genero) || d.Genero.Equals(filtro.Genero, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrEmpty(filtro.Ubicacion) || d.Ubicacion.Equals(filtro.Ubicacion, StringComparison.OrdinalIgnoreCase))
            ).ToList();

            var resultados = new List<object>();
            foreach (var d in destinatariosFiltrados)
            {
                var resultado = await EnviarCorreo(d.Email, d.Nombre);
                resultados.Add(resultado);
            }

            return Ok(new
            {
                TotalEnviados = resultados.Count,
                Resultados = resultados
            });
        }

        //private async Task<object> EnviarCorreo(string email, string nombre)
        //{
        //    var client = new RestClient("https://kq8928.api.infobip.com");
        //    var request = new RestRequest("/email/3/send", Method.Post);

        //    request.AddHeader("Authorization", "App 4d0c4548a64868ddc26011c2dc9b3ff3-8d1301f0-4cc8-4d5f-b975-d267b2a2513c");

        //    request.AddHeader("Content-Type", "multipart/form-data");
        //    request.AddHeader("Accept", "application/json");
        //    request.AlwaysMultipartFormData = true;

        //    request.AddParameter("from", "MiFelicidadMascotas <info@mifelicidadmascotas.com>");
        //    request.AddParameter("subject", "¡Bienvenido a Mi Felicidad Mascotas!");
        //    request.AddParameter("to", $"{{\"to\":\"{email}\",\"placeholders\":{{\"firstName\":\"{nombre}\"}}}}");

        //    var htmlBody = $@"
        //    <html>
        //    <body style='font-family: Arial;'>
        //        <h2 style='color:#2E86C1;'>¡Gracias por unirte a Mi Felicidad Mascotas!</h2>
        //        <p>Hola {nombre},</p>
        //        <p>Muy pronto vas a recibir consejos, novedades y beneficios para vos y tu mascota 🐶🐱.</p>
        //        <a href='https://mifelicidadmascotas.com' style='background-color:#2E86C1;color:white;padding:10px 20px;border-radius:5px;text-decoration:none;'>Visitar el sitio</a>
        //    </body>
        //    </html>";

        //    request.AddParameter("html", htmlBody);
        //    var response = await client.ExecuteAsync(request);

        //    //return new
        //    //{
        //    //    Email = email,
        //    //    StatusCode = response.StatusCode,
        //    //    Success = response.IsSuccessful,
        //    //    Content = response.Content
        //    //};
        //    var content = JsonDocument.Parse(response.Content);
        //    var message = content.RootElement.GetProperty("messages")[0];
        //    var status = message.GetProperty("status");

        //    return new
        //    {
        //        Email = email,
        //        Status = status.GetProperty("name").GetString(),
        //        Description = status.GetProperty("description").GetString()
        //    };


        //}

        private async Task<object> EnviarCorreo(string email, string nombre)
        {
            // Validación simple de entrada
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(nombre))
            {
                return new
                {
                    Email = email,
                    Success = false,
                    Error = "Email o nombre están vacíos."
                };
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
                request.AddParameter("subject", "¡Bienvenido a Mi Felicidad Mascotas!");
                request.AddParameter("to", $"{{\"to\":\"{email}\",\"placeholders\":{{\"firstName\":\"{nombre}\"}}}}");

                var htmlBody = $@"
         <html>
         <body style='font-family: Arial;'>
             <h2 style='color:#2E86C1;'>¡Gracias por unirte a Mi Felicidad Mascotas!</h2>
             <p>Hola {nombre},</p>
             <p>Muy pronto vas a recibir consejos, novedades y beneficios para vos y tu mascota 🐶🐱.</p>
             <a href='https://mifelicidadmascotas.com' style='background-color:#2E86C1;color:white;padding:10px 20px;border-radius:5px;text-decoration:none;'>Visitar el sitio</a>
             <img src='https://localhost:5001/api/tracking/open?correoId=123' width='1' height='1' style='display:none;' />

        </body>
         </html>";

                request.AddParameter("html", htmlBody);

                var response = await client.ExecuteAsync(request);

                if (!response.IsSuccessful)
                {
                    return new
                    {
                        Email = email,
                        Success = false,
                        StatusCode = response.StatusCode,
                        Error = response.Content
                    };
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
                return new
                {
                    Email = email,
                    Success = false,
                    Error = "Excepción: " + ex.Message
                };
            }
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

    }


}
