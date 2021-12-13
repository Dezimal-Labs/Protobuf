using Google.Protobuf.WellKnownTypes;
using Grpc.Net.Client;
using ProductGrpc.Protos;

namespace ProductWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _config;
        private readonly ProductFactory _factory;

        public Worker(ILogger<Worker> logger, IConfiguration config, ProductFactory factory)
        {
            _logger = logger;
            _config = config;
            _factory = factory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Waiting for Productworker_service is running...........");
            Thread.Sleep(2000);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                using var channel = GrpcChannel.ForAddress(_config.GetValue<string>("WorkerService:ServerUrl"));
                var client = new ProductProtoService.ProductProtoServiceClient(channel);
                _logger.LogInformation("AddproductAsync started...");
                var addProductResponse = await client.AddProductAsync(await _factory.Generate());
                _logger.LogInformation("AddProduct Response:{product}",addProductResponse.ToString());  

                //Console.WriteLine("...................................");
                //Console.WriteLine("GetproductAsync service bg Started ....");
                //Console.WriteLine("...................................");
                //var response = await client.GetProductAsync(
                //    new GetProductRequest
                //    {
                //        ProductId = 1
                //    });




                ////Console.WriteLine("GetproductAsync service bg Response :" + response.ToString());

                //Console.WriteLine("...................................");

                //// AddProductAsync
                //Console.WriteLine("AddProductAsync service started...");
                //Console.WriteLine("...................................");
                //var addProductResponse = await client.AddProductAsync(
                //                    new AddProductRequest
                //                    {
                //                        Product = new ProductModel
                //                        {
                //                            //ProductId = 4,
                //                            Name = _config.GetValue<string>("WorkerService:ProductName")+DateTimeOffset.Now,
                //                            Description = "New One plus Phone 1+",
                //                            Price = 1207,
                //                            Status = ProductStatus.Instock,
                //                            CreatedTime = Timestamp.FromDateTime(DateTime.UtcNow)
                //                        }
                //                    });

                //Console.WriteLine("AddProduct service Response: " + addProductResponse.ToString());

                await Task.Delay(_config.GetValue<int>("WorkerService:TaskInterval"), stoppingToken);
               
            }
        }
    }
}