using System;

namespace ApiEnvioMasivo.Models
{
    public class FlujoPaso
    {
        public int Id { get; set; }

        public int FlujoId { get; set; }
        public Flujo Flujo { get; set; }

        public int Orden { get; set; }

        public string Asunto { get; set; }
        public string HtmlContenido { get; set; }

         public int DiasDespuesDelInicio { get; set; }

        public TimeSpan Espera { get; set; } //

        // Tipos: "siempre", "abierto", "no_abierto", "clic"
        public string CondicionTipo { get; set; }

        // Valor opcional para condiciones específicas, como un link
        public string CondicionValor { get; set; }

        public bool Activo { get; set; }

       

       

    }
}
