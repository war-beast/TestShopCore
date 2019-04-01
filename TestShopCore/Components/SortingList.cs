using Microsoft.AspNetCore.Mvc;
using TestShopCore.Models;
using TestShopCore.Repositories;

namespace TestShopCore.Components
{
    public class SortingList : ViewComponent
    {
        UnitOfWork context;
        public SortingList(UnitOfWork db)
        {
            context = db;
        }

        public IViewComponentResult Invoke()
        {
            var sort = new SortType().GetSortTypes();
            return View("_SortingList", sort);
        }
    }
}
