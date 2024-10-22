using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using WebProject.DbContextLayer;
using WebProject.Dto;
using WebProject.Entites;
using WebProject.Extentions;
using WebProject.FileManager;
using WebProject.Services.ProductService;

namespace WebProject.Controllers
{
    public class SearchController : BaseController
    {
        private readonly IFileService _fileService;
        private readonly IProductService _productService;
        public SearchController(ILogger<HomeController> logger,
                                AppDbContext context , 
                                IHttpContextAccessor httpcontext, 
                                IFileService fileService,
                                IProductService productService) : base(logger, context , httpcontext)
        {
            _fileService = fileService;
            _productService = productService;
        }


        [HttpGet]
        [Route("tiem-kiem/{key?}")]
        public async Task<IActionResult> Index([FromRoute] string key)
        {
            if (key == null)
            {
                return RedirectToAction("index", "home");
            }

            var products = new List<Product>();

            if (!string.IsNullOrEmpty(key))
            {

                products = await _context
                            .Products
                            .Include(p => p.Category)
                            .Where(p => p.Title.ToLower().Contains(key.Trim().ToLower()))
                            .Select(p => new Product
                            {
                                ID = p.ID,
                                Name = p.Name,
                                Title = p.Title,
                                Slug = p.Slug,
                                Description = p.Description,
                                Category = p.Category,
                                ImageURL = p.ImageURL

                            }).Take(50).AsNoTracking().ToListAsync();
            }

            foreach (var item in products)
            {
                item.ImageURL = _fileService.HttpContextAccessorPathImgSrcIndex(ProductImg.GetProductImg(), item.ImageURL) ?? "/Image/default.jpg";
            }
            ViewData["key"] = key;

            return View(products);
        }


        [HttpPost]
        public async Task<JsonResult> SearchProduct(string prefix, bool isSearch)
        {
            var format = System.Globalization.CultureInfo.GetCultureInfo("vi-VN");

            var queryProduct = await _context.Products.AsNoTracking().ToListAsync();

            var products = new List<ProductSearchDTO>();

            if (!string.IsNullOrEmpty(prefix))
            {

                products = (await _productService.GetProductsCacheAsync())
                            .Where(p => p.Title.ToLower().Contains(prefix.Trim().ToLower()))
                            .Select(p => new ProductSearchDTO
                            {
                                id = p.ID,
                                title = p.Title,
                                name = p.Name,
                                img = _fileService.HttpContextAccessorPathImgSrcIndex(ProductImg.GetProductImg(), p.ImageURL) ?? "/Image/default.jpg",
                                price = @String.Format(format, "{0:c0}", p.Price),
                                slug = p.Slug,
                            })
                            .Take(50).ToList();
            }

            return Json(products);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitSearch([FromForm] string slug, [FromForm] string key)
        {
            if (slug is null && string.IsNullOrEmpty(key))
            {
                return RedirectToAction("index");
            }

            if (slug != null)
            {
                var product = await _context.Products.Where(p => p.Slug == slug).FirstOrDefaultAsync();

                return RedirectToAction("detail", "home", new { slug = product?.Slug });
            }

            if (!string.IsNullOrEmpty(key))
            {
                return RedirectToAction("index", new { key = key });
            }


            return NotFound($"Lỗi liên hện admin");
        }


        
    }
}
