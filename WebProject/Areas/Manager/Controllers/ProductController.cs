using SixLabors.ImageSharp;

using SixLabors.ImageSharp.Formats.Jpeg;

using SixLabors.ImageSharp.Processing;


using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Identity.Client;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Runtime.CompilerServices;
using WebProject.ATMapper;
using WebProject.DbContextLayer;
using WebProject.Dto;
using WebProject.Entites;
using WebProject.FileManager;
using WebProject.Models;
using WebProject.ModelValidation;
using WebProject.Paging;
using WebProject.Services.CategoryService;
using WebProject.Services.ProductService;
using System.Security.Policy;

namespace WebProject.Areas.Manager.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
       
        private readonly ICategoryService _categoryService;

        private readonly IFileService _fileService;
        public ProductController(AppDbContext appDbContext,
            ILogger<BaseController> logger,
            IProductService productService,
            ICategoryService categoryService,
            IFileService fileService
            ) : base(appDbContext, logger)
        {
            _productService = productService;
            _categoryService = categoryService;
            _fileService = fileService;
        }


        [NonAction]
        public async Task GetDataSeleteCategory()
        {
            var categorys = await _categoryService.GetAllCategoriesAsync();
            categorys = TreeViewModel.GetCategoryChierarchicalTree(categorys);

            var des = new List<Category>();

            TreeViewModel.CreateTreeViewCategorySeleteItems(categorys, des, 0);

            ViewData["SeleteCaterorys"] = des;
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery] ProductParameters productParameter)
        {

            var product = await _productService.GetProductsPaginAsync(productParameter);

            foreach (var item in product.Products)
            {
                if (item.ImageURL is null)
                {
                    item.ImageURL = "/Image/default.jpg";
                }
                else
                {
                    item.ImageURL = _fileService.HttpContextAccessorPathImgSrcIndex(ProductImg.GetProductImg(), item.ImageURL);
                }
               
            }


            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> CreateProduct()
        {
      
            await GetDataSeleteCategory();

            return View("CreateProduct", new ProductCreateDTO() { });
        }


        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductCreateDTO productCreate)
        {
            if (!ModelState.IsValid)
            {
                await GetDataSeleteCategory();
               
                return View("CreateProduct", productCreate);
            }

            var isduplicateId = await _context.Products.AnyAsync(x => x.ID.Equals(productCreate.ID));

            if (isduplicateId)
            {
                await GetDataSeleteCategory();
          
                ModelState.AddModelError(string.Empty, $"Lỗi trùng ID -{productCreate.Slug}- đã tồn tại");

                return View("CreateProduct", productCreate);
            }

            var isduplicateSlug = await _context.Products.AnyAsync(x => x.Slug.Equals(productCreate.Slug));

            if (isduplicateSlug)
            {
                await GetDataSeleteCategory();
               
                ModelState.AddModelError(string.Empty, $"Lỗi trùng SLUG -{productCreate.Slug}- đã tồn tại");

                return View("CreateProduct", productCreate);
            }
            var isDuplicateCode = await IsCodeDuplicateCreate(productCreate.SKU);

            if (!isDuplicateCode)
            {
                await GetDataSeleteCategory();
                

                ModelState.AddModelError(string.Empty, $"Lỗi trùng mã số quản lý kho  -{productCreate.SKU}- đã tồn tại");

                return View("CreateProduct", productCreate);
            }

            try
            {
                var product = ObjectMapper.Mapper.Map<Product>(productCreate);

                await _context.Products.AddAsync(product);

                await _context.SaveChangesAsync();

                StatusMessage = $"Thêm thành công sản phẩm --{product.Name}--.";

                return RedirectToAction("DetailProduct" ,new {id = product.ID});
            }
            catch (Exception)
            {

                await GetDataSeleteCategory();
               
                StatusMessage = $"Lỗi thử lại hoặc liên hệ admin !";

                return View("CreateProduct", productCreate);
            }
        }


        public class InputDetail
        {
            public Product Product { get; set; }

            [BindProperty]
            [FileImgValidation(new string[] { ".jpg", ".jpeg", ".png", ".jfif", ".webp" })]
            [Display(Name = "Ảnh")]
            public IFormFile FormFile { get; set; }
        }

        

        [HttpGet]
        public async Task<IActionResult> DetailProduct([FromRoute] string id)
        {
            var input = new InputDetail();
            input.Product = await _context
                .Products
                .Include(p => p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.ID == id);

            if (input.Product is null)
            {
                return NotFound("Không tìm thấy sản phẩm");
            }

            if (input.Product?.ImageURL is null)
            {
                input.Product.ImageURL = "/Image/default.jpg";
            }
            else
            {
                input.Product.ImageURL = _fileService.HttpContextAccessorPathImgSrcIndex(ProductImg.GetProductImg(), input.Product.ImageURL);
            }
          
            return View("DetailProduct", input);
        }


        public async Task<bool> IsCodeDuplicateCreate(string code)
        {
            if (code == null)
            {
                return true;
            }

            return !(await _context.Products.AsNoTracking().AnyAsync(x => x.SKU.Equals(code)));
        }

        public async Task<bool> IsIdDuplicateUpdate(string key, string keyUpdate)
        {
            if (key == keyUpdate)
            {
                return true;
            }

            return !await _context.Products.AsNoTracking().AnyAsync(x => x.ID.Equals(keyUpdate));

        }
        public async Task<bool> IsSlugDuplicateUpdate(string key, string keyUpdate)
        {
            if (key == keyUpdate)
            {
                return true;
            }

            return !await _context.Products.AsNoTracking().AnyAsync(x => x.Slug.Equals(keyUpdate));


        }
        public async Task<bool> IsCodeDuplicateUpdate(string key, string keyUpdate)
        {
            if (keyUpdate == null)
            {
                return true;
            }

            if (key == keyUpdate)
            {
                return true;
            }

            return !await _context.Products.AsNoTracking().AnyAsync(x => x.ID.Equals(keyUpdate));

        }


        [HttpGet]
        public async Task<IActionResult> UpdateProduct([FromRoute] string id)
        {

            var product = await _context.Products
                .Where(x => x.ID == id).FirstOrDefaultAsync();

            if (product is null)
            {
                return NotFound("Không tìn thấy sản phẩm");
            }

            var productUpdate = ObjectMapper.Mapper.Map<ProductUpdateDTO>(product);

            await GetDataSeleteCategory();

            return View("UpdateProduct", productUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProduct([FromRoute] string id, [FromForm] ProductUpdateDTO productUpdate)
        {
            if (!ModelState.IsValid)
            {
                await GetDataSeleteCategory();
                return View("UpdateProduct", productUpdate);
            }

            var product = await _context.Products.Where(x => x.ID == id).FirstOrDefaultAsync();

            if (product is null)
            {
                await GetDataSeleteCategory();

                return View("UpdateProduct", productUpdate);
            }

            var isSlugDuplicate = await IsSlugDuplicateUpdate(product.Slug, productUpdate.Slug);

            if (!isSlugDuplicate)
            {
                await GetDataSeleteCategory();

                ModelState.AddModelError(string.Empty, $"Lỗi trùng SLUG -{productUpdate.Slug}- đã tồn tại");

                return View("Update", productUpdate);
            }

            var codeDuplicate = await IsCodeDuplicateUpdate(product.SKU, productUpdate.SKU);

            if (!codeDuplicate)
            {
                await GetDataSeleteCategory();

                productUpdate.SKU = product.SKU;
                ModelState.AddModelError(string.Empty, $"Lỗi trùng Mã vạch -{productUpdate.SKU}-");

                return View("Update", productUpdate);
            }
            try
            {
                product.Name = productUpdate.Name;
                product.SKU = product.SKU;
                product.Discount = productUpdate.Discount;

                product.Description = productUpdate.Description;
                product.CostPrice = productUpdate.CostPrice;
                productUpdate.Price = productUpdate.Price;
                product.Title = productUpdate.Title;

                product.CategoryID = productUpdate.CategoryID;

                product.Slug = productUpdate.Slug;
                product.MinimumStock = productUpdate.MinimumStock;

                product.StockQuantity = productUpdate.StockQuantity;
                product.Content = productUpdate.Content;
                product.IsActive = productUpdate.IsActive;

                product.IsFeatured = productUpdate.IsFeatured;

                product.UpdatedDate = DateTime.Now;

                _context.Products.Update(product);

                await _context.SaveChangesAsync();

                StatusMessage = $"Cập nhật thành công sản phẩm {product.Name}";

                return RedirectToAction("DetailProduct", new { id = product.ID });
            }
            catch (Exception)
            {
                return NotFound("Lỗi hệ thống liên hệ admin");
            }

        }

        [HttpPost]
        public async Task<IActionResult> CreateImage([FromForm] InputDetail input , [FromRoute] string id)
        {
            if (input is null || id is null)
            {
                return NotFound("không tìm thấy dữ liệu để nạp");
            }


            var product = await _context.Products.FindAsync(id);

            if (product is null)
            {
                return NotFound($"Không tìm thấy sản phẩm.");
            }

            var FormFile = input.FormFile;

            if (FormFile == null)
            {
                return NotFound("Lỗi không có file để nạp");
            }

            if (ModelState.IsValid)
            {

              
                string Url = FileService.GetUniqueFileWebp();

                var resultFile = await _fileService.CreateFileAsync(ProductImg.GetProductImg(), FormFile, Url);

                var olUrl = product.ImageURL;

                if (resultFile)
                {
                 
                    try
                    {
                        product.ImageURL = Url;
                       
                        await _context.SaveChangesAsync();

                        StatusMessage = $"Thêm Ảnh Thành công";

                        await _fileService.DeleteFileAsync(ProductImg.GetProductImg(), olUrl);

                        return RedirectToAction("DetailProduct", new { id = product.ID });

                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError(string.Empty, "Lỗi hệ thống liên hện admin");

                        return View("DetailProduct", input);
                    }
                }

                await _fileService.DeleteFileAsync(ProductImg.GetProductImg(), olUrl);

            }

            ModelState.AddModelError(string.Empty, "Thêm ảnh không thành công");

            return View("DetailProduct", input);
        }
    }
}