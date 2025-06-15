using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;

namespace ApiEnvioMasivo
{
    public class Program
    {

        public static void Main(string[] args)
        {
          
            //var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";builder.WebHost.UseUrls($"http://*:{port}");
           
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)



                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
