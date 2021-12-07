using ProductGrpc.Models;

namespace ProductGrpc.Data
{
    public class ProductsContextSeed
    {
        public static void SeedAsync(ProductsContext productsContext)
        {
            if(!productsContext.Products.Any())
            {
                var products = new List<Product>
                {
                    new Product {
                        ProductId = 1,
                        Name ="Mi10T",
                        Description ="New Xiomi Phone Mi10T",
                        Price =699,
                        status =ProductGrpc.Models.ProductStatus.INSTOCK,
                        CreatedTime = DateTime.UtcNow
                        },

                      new Product {
                        ProductId = 2,
                        Name ="Mi10T",
                        Description ="New Xiomi Phone Mi10T",
                        Price =699,
                        status =ProductGrpc.Models.ProductStatus.INSTOCK,
                        CreatedTime = DateTime.UtcNow
                        },

                    new Product {
                        ProductId = 3,
                        Name ="A50",
                        Description ="New Samsung Phone A50",
                        Price =699,
                        status =ProductGrpc.Models.ProductStatus.INSTOCK,
                        CreatedTime = DateTime.UtcNow
                        }
                  
                };

                productsContext.Products.AddRange(products);    
                productsContext.SaveChanges();
            }
        }

       
    }
}
