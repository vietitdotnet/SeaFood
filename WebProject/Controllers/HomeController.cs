using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Build.Evaluation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Identity.Client;
using Microsoft.SqlServer.Server;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using WebProject.Areas.Manager.Models;
using WebProject.ATMapper;
using WebProject.DbContextLayer;
using WebProject.Dto;
using WebProject.Entites;
using WebProject.FileManager;
using WebProject.Models;
using WebProject.Paging;
using WebProject.Services.CategoryService;
using WebProject.Services.OrderService;
using WebProject.Services.ProductService;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace WebProject.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ICategoryService _categoryService;
        private readonly IFileService _fileService;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        public HomeController(ILogger<HomeController> logger,
                            AppDbContext context,
                             IHttpContextAccessor httpcontext,
                            ICategoryService categoryService,
                            IFileService fileService,
                            IProductService productService,
                            IOrderService orderService
            ) : base(logger, context, httpcontext)
        {
            _categoryService = categoryService;
            _fileService = fileService;
            _orderService = orderService;
            _productService = productService;
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
                    .Include(p => p.Category)
                    .Where(p => listSerialUrl.Contains(p.CategoryID))
                    .Select( p =>  new Product 
                    {
                        ID = p.ID,
                        Name = p.Name,
                        Title = p.Title,
                        Slug = p.Slug,
                        ImageURL = p.ImageURL,
                        Description = p.Description,
                        Category = p.Category
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

        public class DetailProductModal
        {
            public Product Product { get; set; }

            public OrderInformation OrderInformation { get; set; }
            public List<Product> Products { get; set; }

            public List<SelectSize> SelectSizes { get; set; }
        }

        [HttpGet]
        [Route("san-pham/{slug}")]
        public async Task<IActionResult> Detail([FromRoute] string slug)
        {
            var product = await _context.Products.Where(x=> x.Slug == slug).Include(x => x.Category).FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound("Không tìm thấy loại giống cá");
            }

            var listSerialUrl = new List<string>();

            if (product.Category != null)
            {
                var categorys = await _categoryService.GetCategorysCacheAsync();

                var curentCategory = FindCategoryBySlug(categorys, product.Category.Slug, listSerialUrl);

                if (curentCategory != null)
                {
                    SerialSlugCategorys(curentCategory, listSerialUrl);
                }
              
            }
        
            if (product.ImageURL is null)
            {
                product.ImageURL = "/Image/default.jpg";
            }
            else
            {
                product.ImageURL = _fileService.HttpContextAccessorPathImgSrcIndex(ProductImg.GetProductImg(), product.ImageURL);
            }

            var productModal = new DetailProductModal();

            productModal.Products = await _context
                    .Products
                    .Include(p => p.Category)
                    .Include(p=> p.Specifications)
                    .Where(p => listSerialUrl.Contains(p.Category.Slug))
                    .Select(p => new Product
                    {
                        ID = p.ID,
                        Name = p.Name,
                        Title = p.Title,
                        Slug = p.Slug,
                        ImageURL = p.ImageURL,
                        Description = p.Description,
                        Category = p.Category,
                        Specifications = p.Specifications

                    })
                    .Take(12)
                    .ToListAsync();

            foreach (var productItem in productModal.Products)
            {
                if (productItem.ImageURL is null)
                {
                    productItem.ImageURL = "/Image/default.jpg";
                }
                else
                {
                    productItem.ImageURL = _fileService.HttpContextAccessorPathImgSrcIndex(ProductImg.GetProductImg(), productItem.ImageURL);
                }

            }

            productModal.OrderInformation = new OrderInformation();

            productModal.OrderInformation.ProductID = product.ID;

            productModal.OrderInformation.ProductSlug = product.Slug;

            productModal.Product = product;

            productModal.SelectSizes = new List<SelectSize>();

            if (productModal.Product.Specifications?.Count > 0)
            {
              
                foreach (var item in productModal.Product.Specifications)
                {
                    productModal.SelectSizes.Add(new SelectSize()
                    { ID = item.ID.ToString()
                    , Name = $"Kích thước {item.Size} con / kí giá " + String.Format(System.Globalization.CultureInfo.GetCultureInfo("vi-VN"), "{0:c0}", item.Price)
                    });
                }
            }

            ViewData["curenturl"] = HttpContextAccessorPathDomainFull();

            return View(productModal);
        }

        public class InputGroupModel
        {
            public Category CurentCategory { get; set; }
            public List<Product> RelateProducts { get; set; }
        }

        [HttpGet]
        [Route("danh-muc/{slug}")]
        public async Task<IActionResult> Group([FromRoute]string slug, [FromQuery] ProductParameters productParameter)
        {
            var categorys = await _categoryService.GetCategorysCacheAsync();

            var listSerialUrl = new List<string>();
            
            var curentCategory = FindCategoryBySlug(categorys, slug, listSerialUrl);

            if (curentCategory == null)
            {
                return NotFound("không tìm thấy danh mục");
            }

            var productPagin = await _productService.GetProductsPaginAsync(productParameter, curentCategory.ID);

            var inputModel = new InputGroupModel();
            curentCategory.Products = productPagin.IndexProducts.ToList();

            foreach (var product in curentCategory.Products)
            {
                product.ImageURL = _fileService.HttpContextAccessorPathImgSrcIndex(ProductImg.GetProductImg(), product.ImageURL) ?? "/Image/default.jpg";
            }

            SerialSlugCategorys(curentCategory, listSerialUrl);

            inputModel.CurentCategory = curentCategory;
            
            inputModel.RelateProducts = await _context
                    .Products
                    .Include(p => p.Category)
                    .Where(p => listSerialUrl.Contains(p.Category.Slug))
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
                    .Take(12)
                    .AsNoTracking()
                    .ToListAsync();
            foreach (var product in inputModel.RelateProducts)
            {
                product.ImageURL = _fileService.HttpContextAccessorPathImgSrcIndex(ProductImg.GetProductImg(), product.ImageURL) ?? "/Image/default.jpg";
            }


            return View(inputModel);
        }


        [HttpPost]
        public async Task<IActionResult> SubmitOrderInformation(DetailProductModal productModal )
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);


                var errorMessage = "";
                foreach (var error in errors)
                {
                    // Lấy thông điệp lỗi
                    errorMessage += error.ErrorMessage;
                    // Có thể log lỗi hoặc hiển thị
                    Console.WriteLine(errorMessage);
                }

                // Trả lại view với model và lỗi
                var errorOrder = new ErrorOrder();

                errorOrder.IsSuccess = false;

                errorOrder.Notification = errorMessage;

                errorOrder.ProductSlug = productModal.OrderInformation?.ProductSlug ;

                return RedirectToAction("ErrorOrder", errorOrder);

            }


            try
            {
                var customer = new Customer();

                customer.Address = productModal.OrderInformation.CustomerAddress;

                customer.PhoneNunber = productModal.OrderInformation.PhoneNumber;
                customer.City = productModal.OrderInformation.CustomerCity;
                customer.Name = productModal.OrderInformation.CustomerName;


                var order = new Order();

                order.ID = _orderService.SetId("DH");

                order.ProductID = productModal.OrderInformation.ProductID;

                order.CustomerID = customer.ID;

                order.Description = productModal.OrderInformation.Description;

                order.SpecificationID = productModal.OrderInformation.SpecificationID;

                await  _context.Customers.AddAsync(customer);

                await _context.Orders.AddAsync(order);

                await _context.SaveChangesAsync();

                return RedirectToAction("SuccessOrder", new {id = order.ID});
            }
            catch (Exception)
            {
                var errorOrder = new ErrorOrder();

                errorOrder.IsSuccess = false;

                errorOrder.Notification = "Lỗi quý khách vui lòng thử lại";

                errorOrder.ProductSlug = productModal.OrderInformation?.ProductSlug;
                
                return RedirectToAction("ErrorOrder", errorOrder);
            }

            
            
        }

        [HttpGet]
        public IActionResult ErrorOrder(ErrorOrder errorOrder)
        {
            if (errorOrder == null)
            {
                return RedirectToAction("Index");
            }

            return View(errorOrder);
        }


        [HttpGet]

        public  async Task<IActionResult> SuccessOrder([FromRoute] string id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var order = await _context.Orders.
                Include(x => x.Customer).
                Include(x => x.Product).FirstOrDefaultAsync(x => x.ID == id);

            if (order == null)
            {
                return RedirectToAction("Index");
            }

            if (order.IsRecycleBin || order.IsApproval)
            {
                return RedirectToAction("Index");
            }

            return View(order);

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