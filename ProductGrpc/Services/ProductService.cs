using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using ProductGrpc.Data;
using ProductGrpc.Models;
using ProductGrpc.Protos;

namespace ProductGrpc.Services
{
    public class ProductService : ProductProtoService.ProductProtoServiceBase
    {
        private readonly ProductsContext _productsContext;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;



        public ProductService(ProductsContext productsContext, IMapper mapper, ILogger<ProductService> logger)
        {
            _productsContext= productsContext ?? throw new ArgumentNullException(nameof(productsContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override Task<Empty> Test(Empty request, ServerCallContext context)
        {
            return base.Test(request, context);
        }

        public override async Task<ProductModel> GetProduct(GetProductRequest request, ServerCallContext context)
        {
            var product = await _productsContext.Products.FindAsync(request.ProductId);
            if (product == null)
            {
                //
            }

            //var productModel = new ProductModel
            //{
            //    ProductId = product.ProductId,
            //    Name = product.Name,
            //    Description = product.Description,
            //    Price = product.Price,
            //    Status = ProductStatus.Instock,
            //    CreatedTime = Timestamp.FromDateTime(product.CreatedTime)
            //};

            var productModel = _mapper.Map<ProductModel>(product);

            return productModel;
        }

        public override async Task GetAllProducts(GetAllProductsRequest request, IServerStreamWriter<ProductModel> responseStream, ServerCallContext context)
        {
            var productList = await _productsContext.Products.ToListAsync();

            foreach (var product in productList)
            {
                //var productModel = new ProductModel
                //{
                // ProductId = product.ProductId,
                // Name = product.Name,
                // Description = product.Description,
                // Price = product.Price,
                // Status = ProductStatus.Instock,
                //CreatedTime = Timestamp.FromDateTime(product.CreatedTime)

                var productModel = _mapper.Map<ProductModel>(product);
                await responseStream.WriteAsync(productModel);
            };



        }




        public override async Task<ProductModel> AddProduct(AddProductRequest request, ServerCallContext context)
        {

            //var product = new Product
            //{
            //    ProductId = request.Product.ProductId,
            //    Name = request.Product.Name,
            //    Description = request.Product.Description,
            //    Price = request.Product.Price,
            //    Status = Models.ProductStatus.INSTOCK,
            //    CreatedTime = request.Product.CreatedTime.ToDateTime(),
            //};

            //_productsContext.Products.Add(product);
            //await _productsContext.SaveChangesAsync();


            //var productModel = new ProductModel
            //{
            //    ProductId = product.ProductId,
            //    Name = product.Name,
            //    Description = product.Description,
            //    Price = product.Price,
            //    Status = Protos.ProductStatus.Instock,
            //    CreatedTime = Timestamp.FromDateTime(product.CreatedTime)
            //};
            //return productModel;

            var product = _mapper.Map<Product>(request.Product);

            _productsContext.Products.Add(product);
            await _productsContext.SaveChangesAsync();

            var productModel = _mapper.Map<ProductModel>(product);
            return productModel;
        }
    }

  

}



