using System.Threading.Tasks;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using RestSharp;
using ApiEnvioMasivo.Models;
using Microsoft.EntityFrameworkCore;
using ApiEnvioMasivo.Data;
namespace ApiEnvioMasivo.Services
{
   

    public class InfobipEmailService : IEmailService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;
        private readonly LogService _log;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _db;

        public InfobipEmailService(IHttpClientFactory httpClientFactory, IConfiguration configuration, LogService log, AppDbContext db)
        {
            _httpClientFactory = httpClientFactory;
            _log = log;
            _configuration = configuration;
            _db = db;
        }

        public async Task<string> EnviarCorreoAsync(string to, string subject, string html)
                                      //EnviarCorreo(string email, string nombre, int DestinatarioId, int pasoId)
        {
            var apiKey = _configuration["Infobip:ApiKey"];
            var baseUrl = _configuration["Infobip:BaseUrl"];
            var from = _configuration["Infobip:From"];

            var client = new RestClient($"https://{baseUrl}");
            var request = new RestRequest("/email/3/send", Method.Post);

            request.AddHeader("Authorization", $"App {apiKey}");
            request.AddHeader("Content-Type", "multipart/form-data");
            request.AddHeader("Accept", "application/json");
            request.AlwaysMultipartFormData = true;

            // Parámetros
            request.AddParameter("from", from);
            request.AddParameter("subject", subject);
            request.AddParameter("html", html);
            request.AddParameter("to", $@"{{""to"":""{to}"",""placeholders"":{{""firstName"":""Santi""}}}}");// esto hay que ajustar
            //request.AddParameter("to", $"{{\"to\":\"{email}\",\"placeholders\":{{\"firstName\":\"{nombre}\"}}}}");

            var response = await client.ExecuteAsync(request);

            // Log en la base
            await _log.GuardarLogAsync("INFOBIP_EMAIL", response.Content);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Error al enviar correo: {response.StatusCode} - {response.Content}");
            }

    
            var enviado = new CorreoEnviado
            {
                DestinatarioId = 7,
                FlujoPasoId = 1,
                FechaEnvio = DateTime.UtcNow,
                Abierto = false
            };
            _db.CorreosEnviados.Add(enviado);
            await _db.SaveChangesAsync();

            return "Enviado correctamente";
        }

    }


}
