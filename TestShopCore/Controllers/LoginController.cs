using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TestShop.Models;

namespace TestShop.Controllers
{
    public class LoginController : AsyncController
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Admin/Login
        public async Task<ActionResult> Index()
        {
            if (Request.IsAuthenticated)
                return RedirectToAction("Index", "Profile");

            var admin = await UserManager.FindByNameAsync("test@shop.ru");
            if (admin == null)
                await InitializeAdminAsync();

            return View();
        }

        public ActionResult Registration()
        {
            if (Request.IsAuthenticated)
                return RedirectToAction("Index", "Profile");

            return View();
        }

        private async Task InitializeAdminAsync()
        {
            ApplicationUser admin = new ApplicationUser { Email = "test@shop.ru", PhoneNumber = "+79051111111", UserName = "test@shop.ru" };
            IdentityResult result = await UserManager.CreateAsync(admin, "123456"); //result тут нужен торлько для отладки, так как регистрация админа - это разовая акция и не видна на клиенте.
        }
    }
}