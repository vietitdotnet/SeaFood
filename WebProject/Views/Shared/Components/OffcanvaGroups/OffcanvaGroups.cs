using Microsoft.AspNetCore.Mvc;
using WebProject.Entites;
using WebProject.Services.CategoryService;

namespace WebProject.Views.Shared.Components.OffcanvaGroups
{
    public class OffcanvaGroups : ViewComponent
    {

        private readonly ICategoryService _categoryService;

        public OffcanvaGroups(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public class Menu
        {

            public string CurentCategoryId { get; set; }

            public string CurentSlug { get; set; }

            public List<string> ListSerialUrl { get; set; }

            public IEnumerable<Category> Categories { get; set; }

            public Category CurentCategory { get; set; }


        }

        public const string COMPONENTNAME = "OffcanvaGroups";


        public async Task<IViewComponentResult> InvokeAsync(string slug = null)
        {
            var nemu = new Menu();

            var categorys = await _categoryService.GetCategorysCacheAsync();

            nemu.Categories = categorys;
            if (slug is null)
            {
                
                return View(nemu);
            }

            var listSerialUrl = new List<string>();


            var curentCategory = FindCategoryBySlug(categorys, slug, listSerialUrl);

            if (curentCategory == null)
            {
                return View(nemu);
            }

            nemu.ListSerialUrl = listSerialUrl;
            nemu.CurentSlug = curentCategory.Slug;
            nemu.CurentCategory = curentCategory;
            nemu.CurentCategoryId = curentCategory.ID;

            return View(nemu);

        }

        private void SerialSlugCategorys(Category group, List<string> slugs)
        {

            if (group?.CategoryChildrens?.Count > 0)
            {
                foreach (var item in group.CategoryChildrens)
                {
                    slugs.Add($"{item.Slug}");

                    if (item.CategoryChildrens?.Count > 0)
                    {
                        SerialSlugCategorys(item, slugs);
                    }
                }
            }

        }

        private Category FindCategoryBySlug(IEnumerable<Category> groups, string slug, List<string> slugs)
        {
            try
            {
                foreach (var p in groups)
                {
                    // xử lý cộng nối tiếp các url có trong node

                    slugs.Add(p.Slug);

                    if (p.Slug == slug)
                    {
                        return p;
                    }

                    var p1 = FindCategoryBySlug(p.CategoryChildrens?.ToList() ?? new List<Category>(), slug, slugs);

                    if (p1 != null)

                        return p1;
                }
                slugs.RemoveAt(slugs.Count() - 1);

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
