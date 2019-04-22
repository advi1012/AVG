using System;
using System.Collections.Generic;
using Grpc.Core;
using SupplierServiceGRPC;
using System.Threading.Tasks;
using System.Threading;
using AvG_Abgabe_1___Webapp.Model;

namespace SupplierGRPCClient
{
    class Program
    {
        /// <summary>
        /// Sammlung von Funktionalitäten des Clients
        /// </summary>
        public class SupplierGRPCClient
        {
            readonly Greeter.GreeterClient _client;

            public SupplierGRPCClient(Greeter.GreeterClient client)
            {
                this._client = client;
            }

            // Realisierung von FindAllPreferredSuppliers
            public async Task FindAllPreferredSuppliers()
            {
                Console.WriteLine("***** Task FindAllPreferredSuppliers *****");
                using (var call = _client.FindAllPreferredSuppliers(new Empty()))
                {
                    int i = 1;
                    while (await call.ResponseStream.MoveNext())
                    {
                        Console.WriteLine("***** Task FindAllPreferredSuppliers Iteration " + i + " *****");
                        PreferredSupplier preferredSupplier = call.ResponseStream.Current;
                        Console.WriteLine(i + " Received; preferred supplier: " + preferredSupplier.ToString());
                        Console.WriteLine("***** Iteration " + i + " done *****");
                        i++;
                    }
                }
                Console.WriteLine("***** Task End *****");
            }

            // Realisierung von FindPreferredSupplier
            public async Task FindPreferredSupplier(ProductRequest product)
            {
                var result = await _client.FindPreferredSupplierAsync(product);

                Console.WriteLine("***** Task FindPreferredSupplier ******");
                Console.WriteLine("Supplier:" + result);
                Console.WriteLine("***** Task End *****");
            }

            // Realisierung von setPreferredSupplierForProduct
            public async Task setPreferredSupplierForProduct(setPreferredSupplierForProductRequest x)
            {
                Console.WriteLine("***** Task setPreferredSupplierForProduct *****");
                var result = await _client.setPreferredSupplierForProductAsync(x);

                // 0 ist der Defaultwert für int32 Felder
                if (result.Info != 1)
                {
                    throw new Exception("An unknown error has occured. Please contact the developers for further help.");
                }
            }

            // Sei höflich und begrüße die Users
            public async Task SayHello(string name)
            {
                var reply = await _client.SayHelloAsync(new HelloRequest { Name = name });
                Console.WriteLine("Greeting: " + reply.Message);
            }

            public async Task FindAllSuppliers()
            {
                Console.WriteLine("***** Task FindAllSuppliers *****");
                using (var call = _client.FindAllSuppliers(new Empty()))
                {
                    int i = 1;
                    while (await call.ResponseStream.MoveNext())
                    {
                        Console.WriteLine("***** Task FindAllSuppliers Iteration " + i + " *****");
                        PreferredSupplier supplier = call.ResponseStream.Current;
                        Console.WriteLine(i + " Received; Supplier: " + supplier.ToString());
                        Console.WriteLine("***** Iteration " + i + " done *****");
                        i++;
                    }
                }
                Console.WriteLine("***** Task End *****");
            }

            public async Task FindAllProducts()
            {
                Console.WriteLine("***** Task FindAllProducts *****");
                using (var call = _client.FindAllProducts(new Empty()))
                {
                    int i = 1;
                    while (await call.ResponseStream.MoveNext())
                    {
                        Console.WriteLine("***** Task FindAllProducts Iteration " + i + " *****");
                        ProductRequest product = call.ResponseStream.Current;
                        Console.WriteLine(i + " Received; Product: " + product.ToString());
                        Console.WriteLine("***** Iteration " + i + " done *****");
                        i++;
                    }
                }
                Console.WriteLine("***** Task End *****");
            }
        }

        public static void Main(string[] args)
        {
            Channel channel = new Channel("127.0.0.1:50051", ChannelCredentials.Insecure);
            try
            {
                var client = new SupplierGRPCClient(new Greeter.GreeterClient(channel));
                String welcome = "Welcome to the gRPC client!";

                var reply = client.SayHello(welcome);
                Thread.Sleep(2000);
                Console.WriteLine();
                Console.WriteLine();

                var replyFindAllSup = client.FindAllSuppliers();
                Thread.Sleep(2000);
                Console.WriteLine("Task FindAllSuppliers completed: " + replyFindAllSup.IsCompleted);
                Console.WriteLine();
                Console.WriteLine();

                var replyFindAllPro = client.FindAllProducts();
                Thread.Sleep(2000);
                Console.WriteLine("Task FindAllProducts completed: " + replyFindAllPro.IsCompleted);
                Console.WriteLine();
                Console.WriteLine();

                //Console.WriteLine("Greeting: " + reply.Message);
                var replyFindAll = client.FindAllPreferredSuppliers();
                Thread.Sleep(2000);
                Console.WriteLine("Task FindAllPreferredSuppliers completed: " + replyFindAll.IsCompleted);
                Console.WriteLine();
                Console.WriteLine();

                var replyFindOne = client.FindPreferredSupplier(new ProductRequest { Id = "00000000-0000-0000-0000-000000000001",
                    CurrentStock = 50, Description = "Ich bin Produkt_2", Name = "Produkt_2",
                    Preferredsupplier = "00000000-0000-0000-0000-000000000000", Price = 43.0,
                    Color = ProductRequest.Types.Color.Blue});
                Thread.Sleep(2000);
                Console.WriteLine("Input was Product with Id: 00000000-0000-0000-0000-000000000001");
                Console.WriteLine("Task FindPreferredSupplier completed: " + replyFindOne.IsCompleted);
                Console.WriteLine();
                Console.WriteLine();

                var replyUpdate = client.setPreferredSupplierForProduct(new setPreferredSupplierForProductRequest {
                    PrefSupplier = new PreferredSupplier { Id = "00000000-0000-0000-0000-000000000002", Name = "Alpha", Email = "Alpha@MusterMail.com", Phone = "+49 89 123456 789", Address = "Muster-Adresse" },
                    ProductReq = new ProductRequest { Id = "00000000-0000-0000-0000-000000000001", Preferredsupplier = "00000000-0000-0000-0000-000000000000", Color = ProductRequest.Types.Color.Blue, Price = 43.0, Name = "Produkt_2", Description = "Ich bin Produkt_2", CurrentStock = 50 } });
                Thread.Sleep(2000);
                Console.WriteLine("Input was Product with Id: 00000000-0000-0000-0000-000000000001");
                Console.WriteLine("Input was Supplier with Id: 00000000-0000-0000-0000-000000000000");
                Console.WriteLine("Task setPreferredSupplierForProduct completed: " + replyFindOne.IsCompleted);
                Console.WriteLine();
                Console.WriteLine();
            }
            catch (RpcException e)
            {
                Console.WriteLine(e.Message + ": Es konnte keinen Server gefunden werden.");
            }
            catch (UnknownProductException p)
            {
                Console.WriteLine(p.Message);
            }
            catch (UnknownSupplierException s)
            {
                Console.WriteLine(s.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                channel.ShutdownAsync().Wait();
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}