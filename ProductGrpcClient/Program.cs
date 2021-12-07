﻿// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");


using Grpc.Net.Client;
using ProductGrpc.Protos;


Console.WriteLine("Waiting for server is running");
Thread.Sleep(2000);


using var channel = GrpcChannel.ForAddress("https://localhost:7257");
var client = new ProductProtoService.ProductProtoServiceClient(channel);

//GetProductsAsync

Console.WriteLine("GetproductAsync Started ....");
var response = await client.GetProductAsync(
    new GetProductRequest
    {
        ProductId = 2
    });


Console.WriteLine("GetproductAsync Response :" + response.ToString());
Console.ReadLine();