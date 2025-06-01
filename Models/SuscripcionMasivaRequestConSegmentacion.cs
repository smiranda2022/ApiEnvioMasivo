using System.Collections.Generic;

namespace ApiEnvioMasivo.Models
{
    public class SuscripcionMasivaRequestConSegmentacion
    {
        public List<SuscripcionExtendidaRequest> Destinatarios { get; set; }
        public SegmentacionModel Segmentacion { get; set; }
    }
}
