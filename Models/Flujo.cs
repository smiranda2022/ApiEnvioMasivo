using System;
using System.Collections.Generic;


namespace ApiEnvioMasivo.Models
{
    public class Flujo
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaCreacion { get; set; }

        public ICollection<FlujoPaso> Pasos { get; set; }

        public DateTime? FechaProgramada { get; set; }

        public string HtmlContenido { get; set; }

    }
}
