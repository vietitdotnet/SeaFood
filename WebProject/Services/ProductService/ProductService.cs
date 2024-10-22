using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using WebProject.ATMapper;
using WebProject.DbContextLayer;
using WebProject.Dto;
using WebProject.Entites;
using WebProject.Extentions.Models;
using WebProject.Paging;

namespace WebProject.Services.ProductService
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IMemoryCache _cache;
        public static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        public ProductService(AppDbContext context, ILogger<BaseService> logger , IMemoryCache cache) : base(context, logger)
        {
            _cache = cache;
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

        public async Task<ProductPagin> GetManagerProductsPaginAsync(ProductParameters productParameter)
        {
            var products = await _context.Products
           .OrderBy(p => p.ID)
           .Select(p => new Product
           {
               ID = p.ID,
               Name = p.Name,
               Title = p.Title,
               Slug = p.Slug,
               ImageURL = p.ImageURL,
               Description = p.Description,
               Category = p.Category,
               Specifications = p.Specifications,
           })
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

        public async Task<ProductPagin> GetProductsPaginAsync(ProductParameters productParameter, string categoryId)
        {

            var products = await _context.Products
              .OrderBy(p => p.ID)
              .Where(p => p.CategoryID == categoryId)
              .Select(p => new Product
              {
                  ID = p.ID,
                  Name = p.Name,
                  Title = p.Title,
                  Slug = p.Slug,
                  ImageURL = p.ImageURL,
                  Description = p.Description,
                  Category = p.Category,

              })
               .Skip((productParameter.PageNumber - 1) * productParameter.PageSize)
               .Take(productParameter.PageSize)
               .AsNoTracking()
               .ToListAsync();

            var totalItems = await _context.Products.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)productParameter.PageSize);

            var productPagin = new ProductPagin();

            productPagin.IndexProducts = products;
            productPagin.PageNumber = productParameter.PageNumber;
            productPagin.TotalPages = totalPages;

            return productPagin;

        }

        public async Task<IEnumerable<Product>> GetProductsCacheAsync()
        {
            _logger.Log(LogLevel.Information, "Trying to fetch the list of products from cache.");

            if (_cache.TryGetValue(Cache.keyProduct, out IEnumerable<Product> products))
            {
                _logger.Log(LogLevel.Information, "products list found in cache.");
            }
            else
            {
                try
                {
                    await semaphore.WaitAsync();
                    if (_cache.TryGetValue(Cache.keyProduct, out products))
                    {
                        _logger.Log(LogLevel.Information, "products list found in cache.");
                    }
                    else
                    {
                        _logger.Log(LogLevel.Information, "products list not found in cache. Fetching from database.");


                        products = await _context.Products
                            .Select ( p => new Product
                            {
                                ID = p.ID,
                                Slug = p.Slug,
                                Title = p.Title,
                                Name = p.Name,
                                CreatedDate = p.CreatedDate,
                                UpdatedDate = p.UpdatedDate,
                                IsFeatured = p.IsFeatured,
                                IsActive = p.IsActive,
                                CategoryID = p.CategoryID,
                                ImageURL = p.ImageURL,
                            })
                            .AsNoTracking()
                            .ToListAsync();



                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                                // sẽ hết hạn mục nhập nếu nó không được truy cập trong một khoảng thời gian nhất định.
                                .SetSlidingExpiration(TimeSpan.FromDays(1))
                                //sẽ hết hạn mục sau một khoảng thời gian nhất định.
                                .SetAbsoluteExpiration(TimeSpan.FromDays(3))
                                .SetPriority(CacheItemPriority.High);
                        //.SetSize(1024);
                        _cache.Set(Cache.keyProduct, products, cacheEntryOptions);
                    }
                }
                finally
                {
                    semaphore.Release();
                }
            }
            return products;
        }
    }
}
