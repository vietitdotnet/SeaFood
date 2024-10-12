using Microsoft.EntityFrameworkCore;
using WebProject.ATMapper;
using WebProject.DbContextLayer;
using WebProject.Dto;
using WebProject.Entites;
using WebProject.Paging;

namespace WebProject.Services.ProductService
{
    public class ProductService : BaseService, IProductService
    {
        public ProductService(AppDbContext context, ILogger<BaseService> logger) : base(context, logger)
        {
        }

        public async Task<Product> CreateProductAsync(Product product)
        {
            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return product;
        }

        public async Task DeleteProductAsync(string id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            };
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<ProductPagin> GetProductsPaginAsync(ProductParameters productParameter)
        {
            var products = await _context.Products
           .OrderBy(p => p.ID)
           .Skip((productParameter.PageNumber - 1) * productParameter.PageSize)
            .Take(productParameter.PageSize)
            .ToListAsync();

            var totalItems = await _context.Products.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)productParameter.PageSize);

            var productPagin = new ProductPagin();

            productPagin.Products = ObjectMapper.Mapper.Map<List<ProductDto>>(products);

            productPagin.PageNumber = productParameter.PageNumber;
            productPagin.TotalPages = totalPages;

            return productPagin;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }
    }
}
