using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TestShopCore.Models;
using TestShopCore.Repositories;

namespace TestShopCore.Controllers
{
    public class HomeController : Controller
    {
        UnitOfWork unitOfWork;

        public HomeController(UnitOfWork context)
        {
            unitOfWork = context;
        }

        public IActionResult Index()
        {
            ViewBag.Title = "Тестовый интернет-магазин";

            var categories = unitOfWork.Categories.GetAll();
            var products = new List<Product>();
            foreach(var item in categories)
            {
                products.AddRange(item.Products);
            }

            var model = new ProductListViewModel { Categories = categories, Products = products };
            return View(model);
        }

        public IActionResult SortedProducts()
        {
            var productList = unitOfWork.Products.GetAll();
            return View("_ProductList", productList);
        }

        [HttpPost]
        [Route("api/[controller]/SortedProducts")]
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

        public IActionResult Product(int id = 0)
        {
            if (id == 0)
                return RedirectToAction("Index");

            var product = unitOfWork.Products.Get(id);

            return View(product);
        }

        public IActionResult Category(int id = 0)
        {
            if (id == 0)
                return RedirectToAction("Index");

            var category = unitOfWork.Categories.Get(id);

            return View(category);
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
