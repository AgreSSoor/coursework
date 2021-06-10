using BLLAbstractions.Interfaces;
using Core.DbModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyCourseWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCourseWork.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly IGenericService<User> _genericService;

        private readonly IGenericService<Role> _rolesService;


        public AdminController(IGenericService<User> genericService,
            IGenericService<Role> rolesService)
        {
            _genericService = genericService;
            _rolesService = rolesService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("GetAll");
        }

        public async Task<IActionResult> Details(int id)
        {
            return await GetUserFields(id);
        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewData["RolesList"] = new SelectList(await _rolesService.GetAll(), "Id", "Name");
            return await GetUserFields(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User newUser)
        {
            await _genericService.Update(newUser);
            return RedirectToAction("GetAll");

        }

        public async Task<IActionResult> Delete(int id)
        {
            return  await GetUserFields(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(User user)
        {
            await _genericService.Remove(user);
            return RedirectToAction("GetAll");

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var readResult = await _genericService.GetAll();
            return View(readResult);
        }

        [HttpGet]
        private async Task<IActionResult> GetUserFields(int id)
        {
            var serviceResult = await _genericService.GetById(id);
            return View(serviceResult);
        }
    }
}
