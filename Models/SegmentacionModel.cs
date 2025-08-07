using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEnvioMasivo.Models
{

    public class SegmentacionModel
    {
        public int? Edad { get; set; }
        public string Genero { get; set; }
        public string Ubicacion { get; set; }
        public DateTime? FechaProgramada { get; set; }
    }


    public class Destinatario
    {

        public int Id { get; set; } // ✅ Esto define la clave primaria automáticamente
        public string Email { get; set; }
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public string Genero { get; set; }
        public string Ubicacion { get; set; }

        public int? FlujoId { get; set; } // nullable por si no tiene flujo asignado
        public Flujo Flujo { get; set; }
        public DateTime FechaInicioFlujo { get; set; } // muy importante para evaluar los pasos

    }
}
