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

            return View(categories);
        }

        public IActionResult SortedProducts()
        {
            var productList = unitOfWork.Products.GetAll();
            return View("_ProductList", productList);
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
