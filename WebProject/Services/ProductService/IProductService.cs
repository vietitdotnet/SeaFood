using WebProject.Dto;
using WebProject.Entites;
using WebProject.Paging;

namespace WebProject.Services.ProductService
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(string id);
        Task<Product> CreateProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
        Task DeleteProductAsync(string id);

        Task<ProductPagin> GetProductsPaginAsync(ProductParameters productParameter);
    }
}
