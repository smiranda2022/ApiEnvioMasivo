using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEnvioMasivo.Models
{
    public class SuscripcionExtendidaRequest
    {
        public string Email { get; set; }
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public string Genero { get; set; }
        public string Ubicacion { get; set; }
    }
}
