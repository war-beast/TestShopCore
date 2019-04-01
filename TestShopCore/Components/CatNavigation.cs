using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestShopCore.Repositories;

namespace TestShopCore.Components
{
    public class CatNavigation : ViewComponent
    {
        UnitOfWork context;
        public CatNavigation(UnitOfWork db)
        {
            context = db;
        }

        public IViewComponentResult Invoke()
        {
            var categories = context.Categories.GetAll();
            return View("_CatNavigation", categories);
        }
    }
}
