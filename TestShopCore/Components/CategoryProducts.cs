using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestShopCore.Repositories;

namespace TestShopCore.Components
{
    public class CategoryProducts : ViewComponent
    {
        UnitOfWork context;
        public CategoryProducts(UnitOfWork db)
        {
            context = db;
        }

        public IViewComponentResult Invoke(int id)
        {
            var category = context.Categories.Get(id);
            return View("_CategoryProducts", category);
        }
    }
}
