using Microsoft.AspNetCore.Mvc;
using WebProject.DbContextLayer;
using WebProject.Entites;

namespace WebProject.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ILogger<HomeController> _logger;

        protected readonly AppDbContext _context;

        protected readonly IHttpContextAccessor _httpcontext;
        public BaseController(ILogger<HomeController> logger,
                            AppDbContext context, 
                            IHttpContextAccessor httpcontext)
        {
            _logger = logger;
            _context = context;
            _httpcontext = httpcontext;
        }

        [NonAction]
        protected void SerialSlugCategorys(Category group, List<string> slugs)
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


        [NonAction]
        protected void SerialIdCategorys(Category group, List<string> ids)
        {

            if (group?.CategoryChildrens?.Count > 0)
            {
                foreach (var item in group.CategoryChildrens)
                {
                    ids.Add($"{item.ID}");

                    if (item.CategoryChildrens?.Count > 0)
                    {
                        SerialIdCategorys(item, ids);
                    }
                }
            }

        }

        [NonAction]

        protected Category FindCategoryBySlug(List<Category> groups, string slug, List<string> slugs)
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

        [NonAction]
        protected string HttpContextAccessorPathDomainFull()
        {
            return string.Format("{0}://{1}{2}", _httpcontext.HttpContext.Request.Scheme, _httpcontext.HttpContext.Request.Host.ToString(), _httpcontext.HttpContext.Request.Path);
        }



    }

}

