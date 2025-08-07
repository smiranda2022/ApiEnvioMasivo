using System;

namespace ApiEnvioMasivo.Models
{
    public class CorreoEnviado
    {
        public int Id { get; set; }

        public int DestinatarioId { get; set; }
        public Destinatario Destinatario { get; set; }

        public int FlujoPasoId { get; set; }
        public FlujoPaso FlujoPaso { get; set; }

        public DateTime FechaEnvio { get; set; }
        public bool Abierto { get; set; }

         public DateTime? FechaApertura { get; set; }
        public FlujoPaso Paso { get; set; }

        public bool HizoClic { get; set; }  // para tracking de clics
        public bool Rebotado { get; set; }  // para rebotes



    }
}

