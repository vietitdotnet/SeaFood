using WebProject.Entites;
using Microsoft.AspNetCore.Mvc;

namespace WebProject.Views.Shared.Components.MultiHoverGroups
{
    public class MultiHoverGroups : ViewComponent
    {

        public class Menu
        {

            public string CategoryId { get; set; }

            public string GroupSlug { get; set; }

            public List<string> listSerialUrl { get; set; }

            public IEnumerable<Category> Categories { get; set; }

            public Category Category { get; set; }


        }

        public const string COMPONENTNAME = "MultiHoverGroups";
        public MultiHoverGroups() { }

        public IViewComponentResult Invoke(Menu data)
        {
            return View(data);
        }
    }
}
