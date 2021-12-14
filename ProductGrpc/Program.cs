using Microsoft.EntityFrameworkCore;
using ProductGrpc.Data;
using ProductGrpc.Services;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using ProductGrpc.Protos;
var factory = new ConnectionFactory
{
    //Uri = new Uri("amqp://test:test@localhost:5672")
    Uri = new Uri("amqps://salman:amazonerabbitmq@b-1a6622d5-58dd-41f4-9e71-65ced45d0af5.mq.us-east-2.amazonaws.com:5671")

};



var connection = factory.CreateConnection();
using var channel_rabbit = connection.CreateModel();
channel_rabbit.QueueDeclare("demo-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);


var consumer = new EventingBasicConsumer(channel_rabbit);
consumer.Received += (sender, args) =>
{
    var body = args.Body.ToArray();
   // var message = Encoding.UTF8.GetString(body);

    String jsonified = Encoding.UTF8.GetString(body);
    GetProductRequest response = JsonConvert.DeserializeObject<GetProductRequest>(jsonified);

    

    Console.WriteLine(response);

};

channel_rabbit.BasicConsume("demo-queue", true, consumer);
Console.ReadLine();
var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682


builder.Services.AddTransient<ProductsContextSeed>();
// Add services to the container.

builder.Services.AddDbContext<ProductsContext>(options => options.UseInMemoryDatabase("Products"));

builder.Services.AddGrpc(opt =>
{
    opt.EnableDetailedErrors = true;
});

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
//app.UseSwaggerUI();



//app.UseSwagger();
//app.UseSwaggerUI();


SeedDatabase(app);

void SeedDatabase(IHost app)
{
    //var scopedFactory=app.Services.GetService<IServiceScopeFactory>();
    //using (var scope = scopedFactory.CreateScope())
    //{
    //    var service = scope.ServiceProvider.GetService<ProductsContextSeed>();

    //    service.SeedAsync()
    //}

    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var productContext = services.GetRequiredService<ProductsContext>();
    ProductsContextSeed.SeedAsync(productContext);
}

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();

}

 

//app.MapGrpcService<GreeterService>();
app.MapGrpcService<ProductService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
