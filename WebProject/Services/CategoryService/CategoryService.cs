using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Threading;
using WebProject.Controllers;
using WebProject.DbContextLayer;
using WebProject.Entites;
using WebProject.Extentions.Models;
using WebProject.Models;

namespace WebProject.Services.CategoryService
{
    public class CategoryService : BaseService, ICategoryService
    {
        private readonly IMemoryCache _cache;
        public static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        public CategoryService(AppDbContext context, ILogger<BaseService> logger, IMemoryCache cache) : base(context, logger)
        {
            _cache = cache;
        }

        public async Task<List<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(string id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category> CreateCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task DeleteCategoryAsync(string id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Category>> GetMultiLevelMenuCategory()
        {
            var categories = await _context.Categories.ToListAsync();

            categories = TreeViewModel.GetCategoryChierarchicalTree(categories);


            return categories;
        }

        public async Task<IEnumerable<Category>> GetCategorysCacheAsync()
        {
            _logger.Log(LogLevel.Information, "Trying to fetch the list of Groups from cache.");

            if (_cache.TryGetValue(Cache.keyCategory, out IEnumerable<Category> groups))
            {
                _logger.Log(LogLevel.Information, "Groups list found in cache.");
            }
            else
            {
                try
                {
                    await semaphore.WaitAsync();
                    if (_cache.TryGetValue(Cache.keyCategory, out groups))
                    {
                        _logger.Log(LogLevel.Information, "Groups list found in cache.");
                    }
                    else
                    {
                        _logger.Log(LogLevel.Information, "Groups list not found in cache. Fetching from database.");


                        groups = await _context.Categories.Select(x => new Category
                        {
                            ID = x.ID,
                            Name = x.Name,
                            Slug = x.Slug,
                            ParentCategoryID = x.ParentCategoryID,
                            IsFeatured = x.IsFeatured,
                            CategoryChildrens = x.CategoryChildrens,
                        }).AsNoTracking().ToListAsync();


                        groups = TreeViewModel.GetCategoryChierarchicalTree(groups);

                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                                .SetSlidingExpiration(TimeSpan.FromDays(1))
                                .SetAbsoluteExpiration(TimeSpan.FromDays(3))
                                .SetPriority(CacheItemPriority.High);
                        //.SetSize(1024);
                        _cache.Set(Cache.keyCategory, groups, cacheEntryOptions);
                    }
                }
                finally
                {
                    semaphore.Release();
                }
            }
            return groups;
        }
    }
}
