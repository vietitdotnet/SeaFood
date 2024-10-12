using Microsoft.AspNetCore.Mvc;
using WebProject.Entites;

namespace WebProject.Views.Shared.Components.SliderBoxOwlCarouselProduct
{
    public class SliderBoxOwlCarouselProduct : ViewComponent
    {


        public const string COMPONENTNAME = "SliderBoxOwlCarouselProduct";
        public SliderBoxOwlCarouselProduct() { }

        public IViewComponentResult Invoke(IEnumerable<Product> data)

        {
            return View(data);
        }
    }
   
}
