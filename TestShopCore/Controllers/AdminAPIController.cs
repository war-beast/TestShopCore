using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestShopCore.Models;
using TestShopCore.Repositories;

namespace TestShopCore.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/Admin")]
    public class AdminController : Controller
    {
        UnitOfWork unitOfWork;

        public AdminController(UnitOfWork context)
        {
            unitOfWork = context;
        }

        [HttpPost]
        [Route("CreateCategory")]
        public async Task<IActionResult> CreateCategory([FromBody] Category category)
        {
            string retVal = "ok";

            if (!ModelState.IsValid)
                return BadRequest("Ошибка при заполнении полей формы.");

            await Task.Run(() => {
                unitOfWork.Categories.Create(category);
                unitOfWork.Save();
            });

            return Ok(retVal);
        }

        [HttpPost]
        [Route("EditCategory")]
        public async Task<IActionResult> EditCategory([FromBody] Category category)
        {
            string retVal = "ok";

            if (!ModelState.IsValid)
                return BadRequest("Ошибка при заполнении полей формы.");

            await Task.Run(() => {
                unitOfWork.Categories.Update(category);
                unitOfWork.Save();
            });

            return Ok(retVal);
        }

        [HttpPost]
        [Route("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            string retVal = "ok";

            if (!ModelState.IsValid)
                return BadRequest("Ошибка при заполнении полей формы.");

            await Task.Run(() => {
                unitOfWork.Products.Create(product);
                unitOfWork.Save();
            });

            return Ok(retVal);
        }

        [HttpPost]
        [Route("EditProduct")]
        public async Task<IActionResult> EditProduct([FromBody] Product product)
        {
            string retVal = "ok";

            if (!ModelState.IsValid)
                return BadRequest("Ошибка при заполнении полей формы.");

            await Task.Run(() => {
                unitOfWork.Products.Update(product);
                unitOfWork.Save();
            });

            return Ok(retVal);
        }

        [HttpPost]
        [Route("EditOrderStatus")]
        public async Task<IActionResult> EditOrderStatus([FromBody] Parametr model)
        {
            string retVal = "ok";

            if (!ModelState.IsValid)
            {
                return BadRequest("Не выбрано значение статуса.");
            }

            var order = unitOfWork.Order.Get(model.Id);
            order.State = model.Name;
            unitOfWork.Order.Update(order);
            unitOfWork.Save();

            return Ok(retVal);
        }

        protected override void Dispose(bool disposing)
        {
            unitOfWork.Dispose();
            base.Dispose(disposing);
        }
    }
}
