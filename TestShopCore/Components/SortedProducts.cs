using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestShopCore.Repositories;

namespace TestShopCore.Components
{
    public class SortedProducts : ViewComponent
    {
        UnitOfWork context;
        public SortedProducts(UnitOfWork db)
        {
            context = db;
        }

        public IViewComponentResult Invoke()
        {
            var productList = context.Products.GetAll();
            return View("_ProductList", productList);
        }
    }
}
