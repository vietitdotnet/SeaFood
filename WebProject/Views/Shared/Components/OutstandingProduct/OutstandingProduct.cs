using Microsoft.AspNetCore.Mvc;
using WebProject.Entites;
using WebProject.FileManager;
using WebProject.Services.ProductService;

namespace WebProject.Views.Shared.Components.OutstandingProduct
{
    public class OutstandingProduct : ViewComponent 
    {
        private readonly IProductService _productService;
        private readonly IFileService _fileService;
        public const string COMPONENTNAME = "OutstandingProduct";
        public OutstandingProduct(IProductService productService , IFileService fileService)
        { 
            _productService = productService;
            _fileService = fileService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            
            var products = (await _productService.GetProductsCacheAsync())
                .Where(x => x.IsFeatured == true)
                .Select(p => new Product
                {
                    ID = p.ID,
                    Slug = p.Slug,
                    ImageURL = _fileService.HttpContextAccessorPathImgSrcIndex(ProductImg.GetProductImg(), p.ImageURL) ?? "/Image/default.jpg",
                    Name = p.Name,
                    Title = p.Title,
                  })
                .Take(10).ToList();
               

            return View(products);
        }
    }
}
