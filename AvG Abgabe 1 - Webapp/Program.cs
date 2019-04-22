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
                    // Testdaten werden den Tabellen hinzugefügt
                        context.Supplier.Add(new Supplier("00000000-0000-0000-0000-000000000000", "Alpha", "Alpha@MusterMail.com", "+49 89 123456 789", "Muster-Adresse"));
                        context.Supplier.Add(new Supplier("00000000-0000-0000-0000-000000000001", "Beta", "Beta@MusterMail.com", "+49 89 123456 789", "Muster-Adresse"));
                        context.Supplier.Add(new Supplier("00000000-0000-0000-0000-000000000002", "Gamma", "Gamma@MusterMail.com", "+49 89 123456 789", "Muster-Adresse"));
                        context.Supplier.Add(new Supplier("00000000-0000-0000-0000-000000000003", "Omega", "Omega@MusterMail.com", "+49 89 123456 789", "Muster-Adresse"));
                        context.Supplier.Add(new Supplier("00000000-0000-0000-0000-000000000004", "Epsylon", "Epsylon@MusterMail.com", "+49 89 123456 789", "Muster-Adresse"));
                        context.SaveChanges();
                  
                        context.Product.Add(new Product("00000000-0000-0000-0000-000000000000", "00000000-0000-0000-0000-000000000001", Color.green, 12.0, "Produkt_1", "Ich bin Produkt_1", 100));
                        context.Product.Add(new Product("00000000-0000-0000-0000-000000000001", "00000000-0000-0000-0000-000000000002", Color.blue, 43.0, "Produkt_2", "Ich bin Produkt_2", 50));
                        context.Product.Add(new Product("00000000-0000-0000-0000-000000000002", "00000000-0000-0000-0000-000000000003", Color.red, 100.0, "Produkt_3", "Ich bin Produkt_3", 25));
                        context.SaveChanges();
                    
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
