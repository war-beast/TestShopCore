using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace TestShopCore.Controllers
{
    public class LoginController : Controller
    {
        private readonly SignInManager<IdentityUser> SignInManager;

        public LoginController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            SignInManager = signInManager;
        }

        // GET: Admin/Login
        public IActionResult Index()
        {
            if (SignInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Profile");

            return View();
        }

        public IActionResult Registration()
        {
            if (SignInManager.IsSignedIn(User))
                return RedirectToAction("Index", "Profile");

            return View();
        }
    }
}