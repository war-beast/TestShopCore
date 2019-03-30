using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TestShopCore.Models;

namespace TestShopCore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> UserManager;
        private readonly SignInManager<IdentityUser> SignInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        //api/Account/Logout
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return Ok();
        }

        [AllowAnonymous]
        public IActionResult Login() => View();

        [AllowAnonymous]
        public IActionResult Register() => View();

        // POST api/Account/register
        [AllowAnonymous]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                string retVal = "";
                foreach (var error in ModelState.Values)
                    retVal += error.Errors[0].ErrorMessage + "<br />";
                return BadRequest(retVal);
            }

            var user = new IdentityUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                string retVal = "";
                foreach (var error in result.Errors)
                    retVal += error + "<br />";
                return BadRequest(retVal);
            }

            return Ok();
        }
    }
}
