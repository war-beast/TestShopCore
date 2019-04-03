using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TestShopCore.Models;

namespace TestShopCore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> UserManager;
        private readonly SignInManager<IdentityUser> _signManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            UserManager = userManager;
            _signManager = signInManager;
            _roleManager = roleManager;
        }

        //api/Account/Logout
        [HttpGet]
        [Route("api/[controller]/Logout")]
        public async Task<IActionResult> Logout()
        {
            await _signManager.SignOutAsync();
            return Ok();
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            if (_signManager.IsSignedIn(User))
                return RedirectToAction("Index", "Profile");

            return View();
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            if (_signManager.IsSignedIn(User))
                return RedirectToAction("Index", "Profile");

            return View();
        }

        // POST api/Account/register
        [AllowAnonymous]
        [Route("api/[controller]/register")]
        public async Task Register([FromBody] RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                string retVal = "";
                foreach (var error in ModelState.Values)
                    retVal += error.Errors[0].ErrorMessage + "<br />";
                Response.StatusCode = 400;
                await Response.WriteAsync(retVal);
                return;
            }

            var user = new IdentityUser() { UserName = model.Email, Email = model.Email };

            IdentityResult result = await UserManager.CreateAsync(user, model.Password);


            if (!result.Succeeded)
            {
                string retVal = "";
                foreach (var error in result.Errors)
                    retVal += error.Description + "<br />";
                Response.StatusCode = 400;
                await Response.WriteAsync(retVal);
                return;
            }

            await UserManager.AddToRoleAsync(user, "user");
            Response.StatusCode = 200;
            return;
        }

        [AllowAnonymous]
        [Route("api/[controller]/token")]
        public async Task Token([FromBody] LoginBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                string retVal = "";
                foreach (var error in ModelState.Values)
                    retVal += error.Errors[0].ErrorMessage + "<br />";
                Response.StatusCode = 400;
                await Response.WriteAsync(retVal);
                return;
            }

            var identity = await GetIdentity(model.Login, model.Password);
            if (identity == null)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync("Неправильное имя пользователя или пароль. ");
                return;
            }

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromDays(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = identity.Name
            };

            await _signManager.SignInAsync(await UserManager.FindByNameAsync(model.Login), true);
            // сериализация ответа
            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }

        private async Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            var result = await _signManager.PasswordSignInAsync(username, password, true, false);

            if (result.Succeeded)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, username)
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }
    }
}
