using WebProject.Entites;
using Microsoft.AspNetCore.Mvc;

namespace WebProject.Views.Shared.Components.MultiHoverGroups
{
    public class MultiHoverGroups : ViewComponent
    {

        public class Menu
        {

            public string CurentCategoryId { get; set; }

            public string CurentSlug { get; set; }

            public List<string> ListSerialUrl { get; set; }

            public IEnumerable<Category> Categories { get; set; }

            public Category CurentCategory { get; set; }


        }

        public const string COMPONENTNAME = "MultiHoverGroups";
        public MultiHoverGroups() { }

        public IViewComponentResult Invoke(Menu data)
        {
            return View(data);
        }
    }
}
