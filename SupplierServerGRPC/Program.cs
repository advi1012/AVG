using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Grpc.Core;
using SupplierServiceGRPC;
using AvG_Abgabe_1___Webapp.Service;
using AvG_Abgabe_1___Webapp.Model;
using Microsoft.Extensions.DependencyInjection;

namespace SupplierGRPCServer
{
    /// <summary>
    /// Diese Klasse erbt von der generierten CS.Datei 'SupplerServiceGRPCGrpc.cs'
    /// Jene Datei wurde auf Basis von der .proto Datei beim Building generiert
    /// </summary>
    class GreeterImpl : Greeter.GreeterBase
    {
        // TODO: Datenintegration
        // private readonly IServiceProvider _serviceProvider;

        private BlockingCollection<PreferredSupplier> suppliers;
        private BlockingCollection<ProductRequest> products;
        readonly object myLock = new object();

        public GreeterImpl(//IServiceProvider serviceProvider
           BlockingCollection<PreferredSupplier> suppliers, BlockingCollection<ProductRequest> products)
        {
            // _serviceProvider = serviceProvider;
            this.suppliers = suppliers;
            this.products = products;
        }

        // Server side handler of the SayHello RPC
        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply { Message = request.Name });
        }

        // Server side handler for the SayHelloAgain RPC
        public override Task<HelloReply> SayHelloAgain(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply { Message = request.Name });
        }

        // Server side handler for the FindAllPreferredSuppliers RPC
        public override async Task FindAllPreferredSuppliers(Empty request, IServerStreamWriter<PreferredSupplier> responseStream, ServerCallContext context)
        {
            // Work around für Dependency Injection
            //var scoped = _serviceProvider.CreateScope();
            //var supplierService = scoped.ServiceProvider.GetRequiredService<SupplierService>();
            //List<Supplier> supplierList = supplierService.findAllPreferredSuppliers();
            //foreach(Supplier s in supplierList)
            //{
            //    var p = s.ToPrefferedSupplier();
            //    await responseStream.WriteAsync(p);
            //}
            foreach (var response in suppliers)
            {
                foreach (var product in products)
                {
                    // Nur jene Supplier zurückschreiben, die einen Eintrag in 'preferred supplier' haben
                    if (product.Preferredsupplier == response.Id)
                    await responseStream.WriteAsync(response);
                }
            }
        }

        // Server side handler for the FindPreferredSuppliers RPC
        public override Task<PreferredSupplier> FindPreferredSupplier(ProductRequest request, ServerCallContext context)
        {
            //var scoped = _serviceProvider.CreateScope();
            //var supplierService = scoped.ServiceProvider.GetRequiredService<SupplierService>();
            //Product product = supplierService.findProductById(request.Id);
            //Supplier result = supplierService.findPreferredSupplier(product);
            //return Task.FromResult(result.ToPrefferedSupplier());
            foreach (var supplier in suppliers)
            {
                if (supplier.Id == request.Preferredsupplier)
                {
                    return Task.FromResult(supplier);
                }
            }
            return null;
        }

        // Server side handler for the setPreferredSupplierForProduct RPC
        public override async Task<Empty> setPreferredSupplierForProduct(setPreferredSupplierForProductRequest request, ServerCallContext context)
        {
            //var scoped = _serviceProvider.CreateScope();
            //var supplierService = scoped.ServiceProvider.GetRequiredService<SupplierService>();
            //var supplier = supplierService.findById(request.PrefSupplier.Id);
            //var product = supplierService.findProductById(request.ProductReq.Id);
            //supplierService.setPreferredSupplierForProduct(supplier, product, request.ProductReq.Id);
            //return null;

            if (!products.Contains(request.ProductReq))
            {
                throw new UnknownProductException();
            }

            if (!suppliers.Contains(request.PrefSupplier))
            {
                throw new UnknownSupplierException();
            }

           foreach (var product in products)
            {
                if (product.Id == request.ProductReq.Id)
                {
                    lock (myLock)
                    {
                        Task.Run( () =>
                        product.Preferredsupplier = request.PrefSupplier.Id );
                    }
                    return await Task.FromResult(new Empty { Info = 1 });
                }

            }
            
            return await Task.FromResult(new Empty { Info = 0 });
        }

        // Shows all suppliers in store
        public override async Task FindAllSuppliers(Empty request, IServerStreamWriter<PreferredSupplier> responseStream, ServerCallContext context)
        {
            foreach (var response in suppliers)
            {
                await responseStream.WriteAsync(response);
            }
        }

        // Shows all products in store
        public override async Task FindAllProducts(Empty request, IServerStreamWriter<ProductRequest> responseStream, ServerCallContext context)
        {
            foreach (var response in products)
            {
                await responseStream.WriteAsync(response);
            }
        }
    }

    class Program
    {
        const int Port = 50051;

        public static void Main(string[] args)
        {
            //// Work around für Dependency Injection
            //var collection = new ServiceCollection();
            //collection.AddSingleton<ISupplierService, SupplierService>();
            //var provider = collection.BuildServiceProvider();

            // Testdaten laden, Listen initialisieren...
            var products = new BlockingCollection<ProductRequest>();
            products.Add(new ProductRequest { Id = "00000000-0000-0000-0000-000000000000", Preferredsupplier = "00000000-0000-0000-0000-000000000001", Color = ProductRequest.Types.Color.Green, Price = 12.0, Name = "Produkt_1", Description = "Ich bin Produkt_1", CurrentStock = 100 });
            products.Add(new ProductRequest { Id = "00000000-0000-0000-0000-000000000001", Preferredsupplier = "00000000-0000-0000-0000-000000000002", Color = ProductRequest.Types.Color.Blue, Price = 43.0, Name = "Produkt_2", Description = "Ich bin Produkt_2", CurrentStock = 50 });
            products.Add(new ProductRequest { Id = "00000000-0000-0000-0000-000000000002", Preferredsupplier = "00000000-0000-0000-0000-000000000003", Color = ProductRequest.Types.Color.Red, Price = 100.0, Name = "Produkt_3", Description = "Ich bin Produkt_3", CurrentStock = 25 });

            var suppliers = new BlockingCollection<PreferredSupplier>();
            suppliers.Add(new PreferredSupplier { Id = "00000000-0000-0000-0000-000000000000", Name = "Alpha", Email ="Alpha@MusterMail.com", Phone = "+49 89 123456 789", Address = "Muster-Adresse" });
            suppliers.Add(new PreferredSupplier { Id = "00000000-0000-0000-0000-000000000001", Name = "Beta", Email = "Beta@MusterMail.com", Phone = "+49 89 123456 789", Address = "Muster-Adresse" });
            suppliers.Add(new PreferredSupplier { Id = "00000000-0000-0000-0000-000000000002", Name = "Gamma", Email = "Gamma@MusterMail.com", Phone = "+49 89 123456 789", Address = "Muster-Adresse" });
            suppliers.Add(new PreferredSupplier { Id = "00000000-0000-0000-0000-000000000003", Name ="Omega", Email = "Omega@MusterMail.com", Phone = "+49 89 123456 789", Address = "Muster-Adresse" });
            suppliers.Add(new PreferredSupplier { Id ="00000000-0000-0000-0000-000000000004", Name = "Epsylon", Email ="Epsylon@MusterMail.com", Phone = "+49 89 123456 789", Address = "Muster-Adresse" });

            Server server = new Server
            {
                Services = { Greeter.BindService(new GreeterImpl(suppliers, products)) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
            };
            server.Start();

            Console.WriteLine("Greeter server listening on port " + Port);
            Console.WriteLine("Press any key to stop the server...");
            Console.ReadKey();
            // Lists leeren
            suppliers.Dispose();
            products.Dispose();
            server.ShutdownAsync().Wait();
        }
    }
}
