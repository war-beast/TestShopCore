using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TestShop.Models;
using TestShop.Repositories;

namespace TestShop.Controllers
{
    [Authorize]
    public class ProfileController : AsyncController
    {
        UnitOfWork unitOfWork;
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

        public ProfileController()
        {
            unitOfWork = new UnitOfWork();
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