using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ApiEnvioMasivo.Models
{

    public class SuscripcionRequest
    {
        [Key]
        public int Id { get; set; } // clave primaria

        public string Nombre { get; set; }
        public string Email { get; set; }
    }
}


//using Microsoft.EntityFrameworkCore;


//namespace ApiEnvioMasivo.Models
//{
//    public class AppDbContext : DbContext
//    {
//        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

//        public DbSet<Destinatario> Destinatarios { get; set; }
//    }
//}

