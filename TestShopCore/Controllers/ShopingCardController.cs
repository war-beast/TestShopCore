using Microsoft.AspNetCore.Mvc;

namespace HomeController.Controllers
{
    public class ShopingCardController : Controller
    {
        // GET: ShopingCard
        public IActionResult Index()
        {
            return View();
        }
    }
}