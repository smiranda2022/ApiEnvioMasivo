using System;

namespace ApiEnvioMasivo.Models
{
    public class FlujoHistorial
    {
        public int Id { get; set; }

        public int DestinatarioId { get; set; }
        public Destinatario Destinatario { get; set; }

        public int FlujoId { get; set; }
        public int PasoId { get; set; }

        public DateTime FechaEnvio { get; set; }
        public bool Abierto { get; set; }
        public bool HizoClic { get; set; }
    }
}
