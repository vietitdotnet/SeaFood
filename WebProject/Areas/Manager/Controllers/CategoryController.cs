using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProject.DbContextLayer;
using WebProject.Entites;
using WebProject.Models;
using WebProject.Services.CategoryService;

namespace WebProject.Areas.Manager.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(AppDbContext appDbContext, 
            ILogger<BaseController> logger , 
            ICategoryService categoryService) : base(appDbContext, logger)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var categories = await _categoryService.GetAllCategoriesAsync();

            categories = TreeViewModel.GetCategoryChierarchicalTree(categories);

            
            return View(categories);
        }

        [HttpGet]
        public async  Task<IActionResult> CreateCategory()
        {
            var categorys = await _categoryService.GetAllCategoriesAsync();
            categorys = TreeViewModel.GetCategoryChierarchicalTree(categorys);

            var des = new List<Category>();

            TreeViewModel.CreateTreeViewCategorySeleteItems(categorys, des, 0);

            ViewData["SeleteCaterorys"] = des;

            return View("CreateCategory", new Category());
        }

        public async Task GetDataSeleteCategory()
        {
            var categorys = await _categoryService.GetAllCategoriesAsync();
            categorys = TreeViewModel.GetCategoryChierarchicalTree(categorys);

            var des = new List<Category>();

            TreeViewModel.CreateTreeViewCategorySeleteItems(categorys, des, 0);

            ViewData["SeleteCaterorys"] = des;
        }

        /*bool.Parse(this.IsPrice);*/
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromForm] Category category)
        {
            if (!ModelState.IsValid)
            {
                await GetDataSeleteCategory();

                return View("CreateCategory", category);
            }
            var isduplicateSlug = await _context.Categories.AnyAsync(x => x.Slug.Equals(category.Slug));

            if (isduplicateSlug)
            {
                 await GetDataSeleteCategory();

                ModelState.AddModelError(string.Empty, $"Lỗi trùng Slug -{category.Slug}- đã tồn tại");

                return View("CreateCategory", category);
            }


            try
            {
               
                var result = await _categoryService.CreateCategoryAsync(category);

                StatusMessage = $"Thêm thành công danh mục --{result.Name}--.";

                return RedirectToAction("index");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi thử lại hoặc liên hệ admin");
                await GetDataSeleteCategory();
                return View("Create", category);

            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateCategory([FromRoute] string id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);


            if (category == null)
            {
                return NotFound($"id {id} danh mục không tồn tại");
            }


            await GetDataSeleteCategory();

            return View("UpdateCategory", category);
        }

        [NonAction]
        public async Task<bool> IsDuplicateUpdate(string key, string keyUpdate)
        {
            if (key == keyUpdate)
            {
                return true;
            }

            return !await _context.Categories.AnyAsync(x => x.Slug.Equals(keyUpdate));

        }


        [HttpPost]
        public async Task<IActionResult> UpdateCategory([FromRoute] string id, [FromForm] Category categoryUpdate)
        {

            if (!ModelState.IsValid)
            {
                await GetDataSeleteCategory();

                return View("UpdateCategory", categoryUpdate);
            }
            var category = await _context.Categories.Include(c => c.CategoryChildrens).FirstOrDefaultAsync(c => c.ID == id);


            if (category == null)
            {
                await GetDataSeleteCategory();

                ModelState.AddModelError(string.Empty, $"Không tìm thấy danh mục");

                return View("UpdateCategory", categoryUpdate);
            }

       

            if (category != null)
            {
                if (category.ID == categoryUpdate.ParentCategoryID)
                {
                    await GetDataSeleteCategory();

                    ModelState.AddModelError(string.Empty, $"Lỗi trùng nhóm cha");

                    return View("UpdateCategory", categoryUpdate);
                }
            }


            var isSlugDuplicate = await IsDuplicateUpdate(category.Slug, categoryUpdate.Slug);

            if (!isSlugDuplicate)
            {
                await GetDataSeleteCategory();

                ModelState.AddModelError(string.Empty, $"Lỗi trùng Slug -{categoryUpdate.Slug}- đã tồn tại");

                return View("UpdateCategory", categoryUpdate);
            }


            try
            {
                if (category.CategoryChildrens?.Count > 0)
                {
                    if (category.ParentCategoryID != categoryUpdate.ParentCategoryID)
                    {
                        foreach (var item in category.CategoryChildrens)
                        {
                            item.ParentCategoryID = category.ParentCategoryID;
                        }
                    }
                }

                category.ParentCategoryID = categoryUpdate.ParentCategoryID;
                category.Slug = categoryUpdate.Slug;

                category.Name = categoryUpdate.Name;

                category.IsFeatured = categoryUpdate.IsFeatured;

                _context.Categories.Update(category);

                await _context.SaveChangesAsync();



                StatusMessage = $"Cập nhật thành công danh mục {category.Name}.";

                return RedirectToAction("index");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi thử lại hoặc liên hệ admin");

                return View("UpdateCategory", categoryUpdate);
            }

        }

        [HttpGet]

        public async Task<IActionResult> DeleteCategory([FromRoute] string id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound($"id {id} danh mục không tồn tại");
            }

            return View(category);


        }
        [HttpPost]
        public async Task<IActionResult> DeleteCategory([FromRoute] string id, bool isDelete)
        {
            var category = await _context.Categories.Include(c => c.Products).Include(c => c.CategoryChildrens).FirstOrDefaultAsync(x => x.ID == id);

            if (category == null)
            {
                return NotFound($"id {id} danh mục không tồn tại");
            }

            if (category.Products?.Count > 0)
            {
                ModelState.AddModelError(string.Empty, $"danh mục tồn tại {category.Products?.Count} sản phẩm không thể xóa");

                return View("delete", category);
            }

            try
            {
                if (category.CategoryChildrens?.Count > 0)
                {
                    foreach (var item in category.CategoryChildrens)
                    {
                        item.ParentCategoryID = category.ParentCategoryID;
                    }
                }

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();

                StatusMessage = $"Xóa thành công danh mục --{category.Name}-- .";

                return RedirectToAction("index");

            }
            catch (Exception)
            {
                return NotFound("Lỗi thử lại hoặc liên hệ admin");
            }
        }




    }
}
