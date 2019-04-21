using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using AvG_Abgabe_1___Webapp.Model;
using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace AvG_Abgabe_1___Webapp
{
    public class Program
    {
        // Hier wird der Server bei Ausführung des Programms gestartet
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var context = services.
                        GetRequiredService<SupplierContext>();
                    // Es wird versucht, die Datenbank zu löschen und danch wieder zu erstellen
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, Constants.ClearingDatabaseFailed);
                }
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            //.ConfigureAppConfiguration((hostingContext, config) =>
            //{
            //    // Call other providers here and call AddCommandLine last.
              
            //    config.AddCommandLine(args);
            //})
                .UseStartup<Startup>();
    }
}
