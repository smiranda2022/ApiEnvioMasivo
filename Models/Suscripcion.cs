using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


    namespace ApiEnvioMasivo.Models
    {
        public class Suscripcion
        {
            public int Id { get; set; }
            public string Nombre { get; set; }
            public string Email { get; set; }
            public DateTime Fecha { get; set; } = DateTime.UtcNow;
    }

}