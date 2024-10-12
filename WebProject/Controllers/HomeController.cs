using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using WebProject.ATMapper;
using WebProject.DbContextLayer;
using WebProject.Dto;
using WebProject.Entites;
using WebProject.FileManager;
using WebProject.Models;
using WebProject.Paging;
using WebProject.Services.CategoryService;

namespace WebProject.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IFileService _fileService;
        public HomeController(ILogger<HomeController> logger,
                            AppDbContext context,
                             IHttpContextAccessor httpcontext,
                            ICategoryService categoryService,
                            IFileService fileService
            ) : base(logger, context, httpcontext)
        {
            _categoryService = categoryService;
            _fileService = fileService;
        }

        public class ViewDataModel
        {
            public List<Category> Categories { get; set; }
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetCategorysCacheAsync();

            var listSerialUrl = new List<string>();

            foreach (var category in categories)
            {
                listSerialUrl.Add(category.ID);

                SerialIdCategorys(category, listSerialUrl);

                category.Products = await _context
                    .Products
                    .Where(p => listSerialUrl.Contains(p.CategoryID))
                    .Select( p =>  new Product 
                    {
                        ID = category.ID,
                        Name = p.Name,
                        Title = p.Title,
                        Slug = p.Slug,
                        ImageURL = p.ImageURL,
                        Description = p.Description,
                    })
                   
                    .Take(12)
                    .ToListAsync();
                 

                foreach (var product in category.Products)
                {
                    if (product.ImageURL is null)
                    {
                        product.ImageURL = "/Image/default.jpg";
                    }
                    else
                    {
                        product.ImageURL = _fileService.HttpContextAccessorPathImgSrcIndex(ProductImg.GetProductImg(), product.ImageURL);
                    }
                    

                }

                listSerialUrl.Clear();
            }

          
            return View(categories);

        }

        [HttpGet]
        [Route("danh-muc/{slug?}")]
        public async Task<IActionResult> Detail([FromRoute] string slug)
        {
            var product = await _context .Products.Where(x=> x.Slug == slug).FirstOrDefaultAsync();

            if (product.ImageURL is null)
            {
                product.ImageURL = "/Image/default.jpg";
            }
            else
            {
                product.ImageURL = _fileService.HttpContextAccessorPathImgSrcIndex(ProductImg.GetProductImg(), product.ImageURL);
            }

            ViewData["curenturl"] = HttpContextAccessorPathDomainFull();

            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }


}