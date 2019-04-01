using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestShopCore.Models;
using TestShopCore.Repositories;

namespace TestShopCore.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        UnitOfWork unitOfWork;
        private readonly UserManager<IdentityUser> UserManager;

        public ProfileController(UnitOfWork context, UserManager<IdentityUser> um)
        {
            unitOfWork = context;
            UserManager = um;
        }

        // GET: Profile
        public async Task<ActionResult> Index()
        {
            List<Order> orders = new List<Order>();
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            var customer = unitOfWork.Customer.GetAll().FirstOrDefault(cst => cst.UserId == user.Id);
            if (customer != null) {
                var userOrders = unitOfWork.Order.Find(order => order.CustomerId == customer.Id).OrderByDescending(order => order.Id);
                orders.AddRange(userOrders);
            }

            if(orders.Count > 0)
            {
                for(int idx = 0; idx < orders.Count; idx++)
                {
                    var order = orders[idx];
                    FillProductNames(ref order);
                }
            }

            return View(orders);
        }

        private void FillProductNames(ref Order order)
        {
            foreach(var item in order.OrderItems)
            {
                item.Product = unitOfWork.OrderItem.Get(item.Id).Product;
            }
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}