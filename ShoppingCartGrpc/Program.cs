using Microsoft.EntityFrameworkCore;
using ShoppingCartGrpc.Data;
using ShoppingCartGrpc.Services;
//using ShoppingCartGrpc.Services;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDbContext<ShoppingCartContext>(options => options.UseInMemoryDatabase("ShoppingCart"));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();
SeedDatabase(app);

void SeedDatabase(IHost app)
{

    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var productContext = services.GetRequiredService<ShoppingCartContext>();
    ShoppingCartContextSeed.SeedAsync(productContext);
}


// Configure the HTTP request pipeline.
app.MapGrpcService<ShoppingCartService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
