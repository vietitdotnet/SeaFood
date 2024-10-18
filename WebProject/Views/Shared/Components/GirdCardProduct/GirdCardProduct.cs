using Microsoft.AspNetCore.Mvc;
using WebProject.Entites;

namespace WebProject.Views.Shared.Components.GirdCardProduct
{
 
        public class GirdCardProduct : ViewComponent
        {


            public const string COMPONENTNAME = "GirdCardProduct";
            public GirdCardProduct() { }

            public IViewComponentResult Invoke(IEnumerable<Product> data)

            {
                return View(data);
            }
        }
    
}
