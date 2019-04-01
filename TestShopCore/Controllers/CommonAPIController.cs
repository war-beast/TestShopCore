using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestShopCore.Models;
using TestShopCore.Repositories;

namespace TestShopCore.Controllers
{
    [Route("api/[controller]")]
    public class CommonController : Controller
    {
        private readonly UserManager<IdentityUser> UserManager;
        UnitOfWork unitOfWork;

        public CommonController(UserManager<IdentityUser> userManager, UnitOfWork context)
        {
            unitOfWork = context;
            UserManager = userManager;
        }

        [Authorize]
        [HttpPost]
        [Route("SendShopingCard")]
        public async Task<IActionResult> SendShopingCard([FromBody] ShopingCardViewModel model)
        {
            string retVal = "";
            var customer = await GetCustomer();
            List<OrderItem> orderItems = new List<OrderItem>(model.Items.Count);
            decimal totalSum = 0;

            if(model.Items == null) {
                return BadRequest("Ошибка отправки заказа. Пустой список товаров.");
            }

            foreach (var item in model.Items)
            {
                var prod = unitOfWork.Products.Get(item.Id);
                orderItems.Add(new OrderItem { Product = prod, Count = item.Count });
                totalSum += item.Count * prod.Price;
            }

            var order = new Order
            {
                Customer = customer,
                Address = model.Adress == "" ? "Самовывоз" : model.Adress,
                Date = DateTime.Now,
                OrderItems = orderItems,
                State = "",
                Sum = totalSum
            };
            unitOfWork.Order.Create(order);

            try
            {
                unitOfWork.Save();
            }
            catch (Exception)
            {
                retVal = "На сервере произошла ошибка при обработке заказа.";
                return BadRequest(retVal);
            }

            return Ok(retVal);
        }

        [HttpPost]
        [Route("SendReview")]
        public IActionResult SendReview([FromBody] ReviewViewModel model)
        {
            string retVal = "ok";
            if (!ModelState.IsValid)
                return BadRequest("Правильно заполните все поля.");

            var product = unitOfWork.Products.Get(model.ProductId);
            var rating = Math.Round((double)(product.Rating + model.Value) / 2);
            product.Rating = (int)rating;
            unitOfWork.Save();

            return Ok(retVal);
        }

        [HttpPost]
        [Route("SortedProducts")]
        public IActionResult SortedProducts([FromBody] FilterViewModels filter)
        {
            IEnumerable<Product> productList = new List<Product>();
            var allProducts = unitOfWork.Products.GetAll();

            foreach (var category in filter.Categories)
            {
                var query = allProducts.Where(prod => prod.Price >= filter.MinPrice && prod.Price <= filter.MaxPrice && prod.CategoryId == category.Id);
                productList = productList.Concat(query);
                var list = new List<Product>();
                list.AddRange(productList);
            }


            switch (filter.Sort)
            {
                case 1:
                    productList = productList.OrderBy(pr => pr.Price);
                    break;
                case 2:
                    productList = productList.OrderBy(pr => pr.Rating);
                    break;
            }
            return PartialView("_ProductList", productList);
        }

        #region Вспомогательные методы
        private async Task<Customer> GetCustomer()
        {
            var user = await UserManager.FindByNameAsync(User.Identity.Name);
            var customer = unitOfWork.Customer.GetAll().FirstOrDefault(cst => cst.UserId == user.Id);
            if (customer == null)
            {
                unitOfWork.Customer.Create(new Customer { UserId = user.Id });
                unitOfWork.Save();
                customer = unitOfWork.Customer.GetAll().FirstOrDefault(cst => cst.UserId == user.Id);
            }

            return customer;
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
