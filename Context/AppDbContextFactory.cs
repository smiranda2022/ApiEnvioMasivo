using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiEnvioMasivo.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

//namespace ApiEnvioMasivo
//{
//    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
//    {
//        public AppDbContext CreateDbContext(string[] args)
//        {
//            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
//            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EnvioMasivoDb;Trusted_Connection=True;");

//            return new AppDbContext(optionsBuilder.Options);
//        }
//    }
//}

using Microsoft.Extensions.Configuration;
using System.IO;

namespace ApiEnvioMasivo.Data
{
    public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
