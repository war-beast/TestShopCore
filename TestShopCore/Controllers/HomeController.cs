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

        public ActionResult Index()
        {
            ViewBag.Title = "Тестовый интернет-магазин";

            var categories = unitOfWork.Categories.GetAll();

            return View(categories);
        }

        [HttpPost]
        public ActionResult SortedProducts(FilterViewModels filter)
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

        public ActionResult CategoryProducts(int id = 0)
        {
            if (id == 0)
                return RedirectToAction("Index");

            var category = unitOfWork.Categories.Get(id);
            return PartialView("_CategoryProducts", category);
        }

        public ActionResult Product(int id = 0)
        {
            if (id == 0)
                return RedirectToAction("Index");

            var product = unitOfWork.Products.Get(id);

            return View(product);
        }

        public ActionResult Category(int id = 0)
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
