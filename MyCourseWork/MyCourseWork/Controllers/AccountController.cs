using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BLLAbstractions.Interfaces;
using Core.AuthModels;
using Core.DbModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCourseWork.Models;

namespace MyCourseWork.Controllers
{
    public class AccountController : Controller
    {

        private readonly IGenericService<User> _genericService;
        
        private readonly IGenericService<Role> _rolesService;
        
        public AccountController(IGenericService<User> genericService, IGenericService<Role> rolesService)
        {
            _genericService = genericService;
            _rolesService = rolesService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var users = await _genericService.GetAll();
                    await _rolesService.GetAll();
                    User user = users
                        .FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                if (user != null)
                {
                    await Authenticate(user); // аутентификация

                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await _genericService.FirstOrDefault(u => u.Email == model.Email);
                if (user == null)
                {
                    user = new User { Name = model.Name, Surname = model.Surname, Email = model.Email, Password = model.Password, RoleId = 1};

                    await _genericService.Add(user);

                    Role userRole = await _rolesService.FirstOrDefault(r => r.Id == 1);
                    if (userRole != null)
                    {
                        user.Role = userRole;
                    }

                    await Authenticate(user);

                    return RedirectToAction("Index", "Home");
                }
                
                ModelState.AddModelError("", "Пользователь с таким e-mail уже существует");
            }

            return View();
        }
        
        public IActionResult Restrict()
        {
            ViewBag.Message = "Недостаточно прав";
            return View();
        }
        
        private async Task Authenticate(User user)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
