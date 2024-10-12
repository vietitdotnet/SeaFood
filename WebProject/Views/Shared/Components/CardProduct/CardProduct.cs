using Microsoft.AspNetCore.Mvc;
using WebProject.Entites;

namespace WebProject.Views.Shared.Components.CardProduct
{
    public class CardProduct : ViewComponent
    {


        public const string COMPONENTNAME = "CardProduct";
        public CardProduct() { }

        public IViewComponentResult Invoke(Product data)
        {
            return View(data);
        }
    }
}
