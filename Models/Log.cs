using System;

namespace ApiEnvioMasivo.Models
{
    public class Log
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; }        // ej: "INFOBIP", "ERROR", etc.
        public string Contenido { get; set; }   // JSON, texto, etc.
    }
}

