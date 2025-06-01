using ApiEnvioMasivo.Data;
using ApiEnvioMasivo.Models;
using System;
using System.Linq;
using ApiEnvioMasivo.Models;
using System.Collections.Generic;

namespace ApiEnvioMasivo.Seed
{
    public static class FlujoSeeder
    {
        public static void CargarFlujoDeEjemplo(AppDbContext db)
        {
            if (db.Flujos.Any()) return;

            var flujo = new Flujo
            {
                Nombre = "Bienvenida Mascotas",
                Descripcion = "Secuencia para nuevos suscriptores",
                Activo = true,
                FechaCreacion = DateTime.UtcNow,
                Pasos = new List<FlujoPaso>
                {
                    new FlujoPaso
                    {
                        Orden = 1,
                        Asunto = "¡Bienvenido a Mi Felicidad Mascotas!",
                        HtmlContenido = "<p>Hola {{nombre}}, gracias por sumarte 💌</p>",
                        //DiasDespuesDelInicio = 0,
                        Espera = TimeSpan.Zero,
                        CondicionTipo = "siempre",
                        Activo = true
                    },
                    new FlujoPaso
                    {
                        Orden = 2,
                        Asunto = "¿Aún no conociste nuestros beneficios?",
                        HtmlContenido = "<p>Pasaron 3 días, {{nombre}}. ¿Querés ver lo que tenemos para vos?</p>",
                       // DiasDespuesDelInicio = 0,// cambiar cuando sea necesario
                        Espera = TimeSpan.FromMinutes(5),
                        CondicionTipo = "no_abierto",
                        Activo = true
                    }
                }
            };

            db.Flujos.Add(flujo);
            db.SaveChanges();
        }
    }
}
