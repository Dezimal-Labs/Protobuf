using Microsoft.EntityFrameworkCore;
using ProductGrpc.Data;
using ProductGrpc.Services;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682


builder.Services.AddTransient<ProductsContextSeed>();
// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddDbContext<ProductsContext>(options => options.UseInMemoryDatabase("Products"));
var app = builder.Build();

SeedDatabase(app);

void SeedDatabase(IHost app)
{
    //var scopedFactory=app.Services.GetService<IServiceScopeFactory>();
    //using (var scope = scopedFactory.CreateScope())
    //{
    //    var service = scope.ServiceProvider.GetService<ProductsContextSeed>();
        
    //    service.SeedAsync()
    //}

    using var scope=app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var productContext=services.GetRequiredService<ProductsContext>();
    ProductsContextSeed.SeedAsync(productContext);
}



// Configure the HTTP request pipeline.
//app.MapGrpcService<GreeterService>();
app.MapGrpcService<ProductService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
