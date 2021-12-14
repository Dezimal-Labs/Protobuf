// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");


using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using Newtonsoft.Json;
using ProductGrpc.Protos;
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory
{
    //Uri = new Uri("amqp://test:test@localhost:5672")
    Uri = new Uri("amqps://salman:amazonerabbitmq@b-1a6622d5-58dd-41f4-9e71-65ced45d0af5.mq.us-east-2.amazonaws.com:5671")

};

var connection = factory.CreateConnection();
using var channel_rabbit = connection.CreateModel();
channel_rabbit.QueueDeclare("demo-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

using var channel = GrpcChannel.ForAddress("https://localhost:7257");
var client = new ProductProtoService.ProductProtoServiceClient(channel);
//await GetAllProducts(client);
//await GetProductAsync(client);
//await GetProd(client);
//await AddProductAsync(client);
//await GetAllProducts(client);

//await UpdateProductAsync(client);
//await GetAllProducts(client);

SendMessageToRabbitMQ(client);

void SendMessageToRabbitMQ(ProductProtoService.ProductProtoServiceClient client)
{
    Console.WriteLine("...................................");
    Console.WriteLine("Rabbitmq Started ....");
    Console.WriteLine("...................................");


    var request = new GetProductRequest
    {
        ProductId = 1
    };


    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));
    channel_rabbit.BasicPublish("", "demo-queue", null, body);
}

static async Task  GetProd(ProductProtoService.ProductProtoServiceClient client)
{

    Console.WriteLine("...................................");
    Console.WriteLine("Getprod Started ....");
    Console.WriteLine("...................................");



    var response = await client.GetProductAsync(
         new GetProductRequest
         {
             ProductId = 1
         });


    Console.WriteLine("Getprod Response :" + response.ToString());
}


//await DeleteProductAsync(client);
//await GetAllProducts(client);

//await InsertBulkProduct(client);
//await GetAllProducts(client);

//insert bulk products
static async Task InsertBulkProduct(ProductProtoService.ProductProtoServiceClient client)
{
    Console.WriteLine("...................................");
    Console.WriteLine("InsertBulkProduct Started ....");
    Console.WriteLine("...................................");
    using var clientBulk = client.InsertBulkProduct();

    for (int i = 0; i < 10; i++)
    {
        var productModel = new ProductModel
        {
                Name = $"Product_{i}",
                Description = $"Bulk inserted product_{i}",
                Price = 1207+ i ,
                Status = ProductStatus.Instock,
                CreatedTime = Timestamp.FromDateTime(DateTime.UtcNow)
          };

        await clientBulk.RequestStream.WriteAsync(productModel);
    }
    await clientBulk.RequestStream.CompleteAsync(); ;
    var responseBulk = await clientBulk;
    Console.WriteLine($"Status : {responseBulk.Success}. Insert count : {responseBulk.InsertCount}");
}

Console.ReadLine();




//GetProductsAsync
static async Task GetProductAsync(ProductProtoService.ProductProtoServiceClient client)
{
    Console.WriteLine("...................................");
    Console.WriteLine("GetproductAsync Started ....");
    Console.WriteLine("...................................");



    //var response = await client.GetProductAsync(
    //     new GetProductRequest
    //     {
    //         ProductId = 1
    //     });


    //Console.WriteLine("GetproductAsync Response :" + response.ToString());


    var request = new GetProductRequest
                     {
                            ProductId = 1
                       };


    var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(request));

    var response = await client.GetProductAsync(request);


    Console.WriteLine("GetproductAsync Response :" + response.ToString());

}


//GetAllproducts
async Task GetAllProducts(ProductProtoService.ProductProtoServiceClient client)
{

    //GetAllproducts products server stream Method from client
    Console.WriteLine("...................................");
    Console.WriteLine("GetAllproducts Started ....");
    Console.WriteLine("...................................");

    using (var clientData = client.GetAllProducts(new GetAllProductsRequest()))
    {
        while (await clientData.ResponseStream.MoveNext(new System.Threading.CancellationToken()))
        {
            var currentProduct = clientData.ResponseStream.Current;
            Console.WriteLine(currentProduct);
        }
    }

    Thread.Sleep(2000);

    ////GetAllProducts with c# 9

    //Console.WriteLine("GetAllproducts with c# 9 Started ....");
    //using var clientData = client.GetAllProducts(new GetAllProductsRequest());
    //await foreach (var responseData in clientData.ResponseStream.ReadAllAsync())
    //{
    //    Console.WriteLine(responseData);
    //}

}



//Add product
static async Task AddProductAsync(ProductProtoService.ProductProtoServiceClient client)
{
    Console.WriteLine("...................................");

    // AddProductAsync
    Console.WriteLine("AddProductAsync started...");
    Console.WriteLine("...................................");
    var addProductResponse = await client.AddProductAsync(
                        new AddProductRequest
                        {
                            Product = new ProductModel
                            {
//                                ProductId = ,
                                Name = "One Plus",
                                Description = "New One plus Phone 1+",
                                Price = 1207,
                                Status = ProductStatus.Instock,
                                CreatedTime = Timestamp.FromDateTime(DateTime.UtcNow)
                            }
                        });

    Console.WriteLine("AddProduct Response: " + addProductResponse.ToString());
    Thread.Sleep(2000);
}




//Update product
static async Task UpdateProductAsync(ProductProtoService.ProductProtoServiceClient client)
{
    Console.WriteLine("...................................");
    Console.WriteLine("UpdateProductAsync Started ....");
    Console.WriteLine("...................................");
    var updateProductResponse = await client.UpdateProductAsync(
        new UpdateProductRequest
        {
            Product = new ProductModel
            {
                ProductId = 1,
                Name = "One Plus",
                Description = "New Xiomi Phone Mi10T",
                Price = 1207,
                Status = ProductStatus.Instock,
                CreatedTime = Timestamp.FromDateTime(DateTime.UtcNow)
            }
        });
    Console.WriteLine("UpdateProductAsync Response: " + updateProductResponse.ToString());
}


//delete product
static async Task DeleteProductAsync(ProductProtoService.ProductProtoServiceClient client)
{
    Console.WriteLine("...................................");
    Console.WriteLine("DeleteProductAsync Started ....");
    Console.WriteLine("...................................");
    var deleteProductResponse = await client.DeleteProductAsync(
        new DeleteProductRequest
        {
            ProductId = 3,
        }

        );

    Console.WriteLine("DeleteProductAsync Response :" + deleteProductResponse.Success.ToString());
    Thread.Sleep(1000);
}




