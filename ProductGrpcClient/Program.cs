// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");


using Grpc.Core;
using Grpc.Net.Client;
using ProductGrpc.Protos;

using var channel = GrpcChannel.ForAddress("https://localhost:7257");
var client = new ProductProtoService.ProductProtoServiceClient(channel);

await GetAllProducts(client);

await GetProductAsync(client);


//GetAllproducts
async Task GetAllProducts(ProductProtoService.ProductProtoServiceClient client)
{

   // //GetAllproducts products server stream Method from client
    //Console.WriteLine("GetAllproducts Started ....");
    //using (var clientData = client.GetAllProducts(new GetAllProductsRequest()))
    //{
    //    while (await clientData.ResponseStream.MoveNext(new System.Threading.CancellationToken()))
    //    { 
    //        var currentProduct=clientData.ResponseStream.Current;
    //        Console.WriteLine(currentProduct);
    //    }
    //}

    ////GetAllProducts with c# 9

    Console.WriteLine("GetAllproducts with c# 9 Started ....");
    using var clientData = client.GetAllProducts(new GetAllProductsRequest());
    await foreach (var responseData in clientData.ResponseStream.ReadAllAsync())
    {
        Console.WriteLine(responseData);
    }

}

//GetProductsAsync
static async Task GetProductAsync(ProductProtoService.ProductProtoServiceClient client)
{
    Console.WriteLine("GetproductAsync Started ....");
    var response = await client.GetProductAsync(
        new GetProductRequest
        {
            ProductId = 2
        });


    Console.WriteLine("GetproductAsync Response :" + response.ToString());
    Console.WriteLine("Waiting for server is running");
    Thread.Sleep(2000);
}


Console.ReadLine();