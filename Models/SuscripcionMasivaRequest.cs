using System.Collections.Generic;

namespace ApiEnvioMasivo.Models
{
    public class SuscripcionMasivaRequest
    {
        public List<SuscripcionRequest> Destinatarios { get; set; }
    }
}