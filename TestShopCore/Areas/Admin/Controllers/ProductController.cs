using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TestShopCore.Models;
using TestShopCore.Repositories;
using TestShopCore.Areas.Admin.Models;

namespace TestShopCore.Areas.Admin.Controllers
{
    [Authorize(Roles = "admin")]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly UserManager<IdentityUser> UserManager;
        private readonly UnitOfWork unitOfWork;

        public ProductController(UserManager<IdentityUser> userManager, UnitOfWork context)
        {
            UserManager = userManager;
            unitOfWork = context;
        }

        public ActionResult Index(int? id)
        {
            int identity = 0;
            if (id.HasValue)
                identity = id.Value;

            var category = unitOfWork.Categories.Get(identity);
            var products = category == null ? new List<Product>() : category.Products;
            var model = new AdminProductListViewModel { Id = id, CategoryName = category?.Name, Products = products };
            return View(model);
        }

        public ActionResult Details(int? id)
        {
            int identity = 0;
            if (id.HasValue)
                identity = id.Value;

            var product = unitOfWork.Products.Get(identity);
            return PartialView("_Details", product);
        }

        public ActionResult Create(int? id)
        {
            int identity = 0;
            if (id.HasValue)
                identity = id.Value;

            var category = unitOfWork.Categories.Get(identity);
            var newProduct = new Product { CategoryId = category.Id };
            return PartialView("_Create", newProduct);
        }

        public ActionResult Edit(int? id)
        {
            int identity = 0;
            if (id.HasValue)
                identity = id.Value;

            var product = unitOfWork.Products.Get(identity);
            return PartialView("_Edit", product);
        }

        public ActionResult Delete(int? id)
        {
            int identity = 0;
            if (id.HasValue)
                identity = id.Value;

            var product = unitOfWork.Products.Get(identity);
            return PartialView("_Delete", product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteRecord(Product product)
        {
            unitOfWork.Products.Delete(product.Id);
            unitOfWork.Save();
            return RedirectToAction("Index", new { Id = product.CategoryId });
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}