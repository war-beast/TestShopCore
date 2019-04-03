using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestShopCore.Areas.Admin.Utils;
using TestShopCore.Areas.Admin.Models;
using TestShopCore.Repositories;

namespace TestShopCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    [Route("/Admin/[controller]/")]
    public class OrderController : Controller
    {
        private readonly UserManager<IdentityUser> UserManager;
        private readonly UnitOfWork unitOfWork;

        public OrderController(UserManager<IdentityUser> userManager, UnitOfWork context)
        {
            UserManager = userManager;
            unitOfWork = context;
        }

        // GET: Admin/Order
        public async Task<IActionResult> Index()
        {
            List<OrderViewModel> orders = new List<OrderViewModel>();
            var allOrders = unitOfWork.Order.GetAll().OrderByDescending(ord => ord.Id);

            if (allOrders.Count() > 0)
            {
                for (int idx = 0; idx < allOrders.Count(); idx++)
                {
                    var order = allOrders.ElementAt(idx);
                    var userId = order.Customer.UserId;
                    var user = await UserManager.FindByIdAsync(userId);
                    orders.Add(new OrderViewModel { CustomerEmail = user.Email, Order = order });
                }
            }

            return View(orders);
        }

        [Route("EditStatus")]
        public IActionResult EditStatus(int id = 0)
        {
            var order = unitOfWork.Order.Get(id);
            ViewBag.StatusList = OrderState.GelList();

            return PartialView("_EditStatus", order);
        }

        [Route("Products")]
        public IActionResult Products(int id = 0)
        {
            var order = unitOfWork.Order.Get(id);
            foreach (var item in order.OrderItems)
            {
                item.Product = unitOfWork.OrderItem.Get(item.Id).Product;
            }

            return PartialView("_Products", order);
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}