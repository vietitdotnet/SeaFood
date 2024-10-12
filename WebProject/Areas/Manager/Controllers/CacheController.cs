using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using WebProject.Areas.Manager.Models;
using WebProject.DbContextLayer;
using WebProject.Extentions.Models;
using WebProject.Services.CategoryService;

namespace WebProject.Areas.Manager.Controllers
{
    public class CacheController : BaseController
    {
        private readonly IMemoryCache _cache;

        private readonly ICategoryService _categoryService;
  
        public CacheController(AppDbContext appDbContext, ILogger<BaseController> logger, IMemoryCache memoryCache, ICategoryService categoryService) : base(appDbContext, logger)
        {
            _cache = memoryCache;
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Index()
        {
         return View(new Cache());
        }


        [HttpPost]
        public async Task<IActionResult> Update()
        {
            try
            {
                await UpdateCache();
                StatusMessage = $"Cập nhật cache thành công";
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                StatusMessage = $"Cập nhật không thành công";
                return View("Index");
            }

        }

        [NonAction]
        public async Task UpdateCache()
        {

            var task = new Task(() =>
            {
                _cache.Remove(Cache.keyProduct);
                _cache.Remove(Cache.keyCategory);
                _cache.Remove(Cache.keyPolicy);

            });

            task.Start();
            await task;
         
           /* await _categoryService.GetCategorysCacheAsync();*/

            /*await GetProducts();
            await GetPolicys();*/
        }
    }
}
