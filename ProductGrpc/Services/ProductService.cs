using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ProductGrpc.Data;
using ProductGrpc.Protos;

namespace ProductGrpc.Services
{
    public class ProductService : ProductProtoService.ProductProtoServiceBase
    {
        private readonly ProductsContext _productsContext;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ProductsContext productsContext, ILogger<ProductService> logger)
        {
            _productsContext = productsContext ?? throw new ArgumentNullException(nameof(productsContext));
            // _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        public override async Task<ProductModel> GetProduct(GetProductRequest request, ServerCallContext context)
        {
            var product = await _productsContext.Products.FindAsync(request.ProductId);
            if (product == null)
            {
                //
            }

            var productModel = new ProductModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Status = ProductStatus.Instock,
                CreatedTime = Timestamp.FromDateTime(product.CreatedTime)
            };

            return productModel;
        }

        public override async Task GetAllProducts(GetAllProductsRequest request, IServerStreamWriter<ProductModel> responseStream, ServerCallContext context)
        {
            var productList = await _productsContext.Products.ToListAsync();

            foreach (var product in productList)
            {
                var productModel = new ProductModel
                {
                    ProductId = product.ProductId,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Status = ProductStatus.Instock,
                   CreatedTime = Timestamp.FromDateTime(product.CreatedTime)
                };

                await responseStream.WriteAsync(productModel);

            }
            //return base.GetAllProducts(request, responseStream, context);
        }
    }
}
