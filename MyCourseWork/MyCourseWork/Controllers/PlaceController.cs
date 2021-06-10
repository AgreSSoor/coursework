using BLLAbstractions.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyCourseWork.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Core.DbModels;

namespace MyCourseWork.Controllers
{
    public class PlaceController : Controller
    {
        private readonly IGenericService<Place> _genericService;

        public PlaceController(IGenericService<Place> genericService)
        {
            _genericService = genericService;
        }

        public IActionResult Index()
        {
            return RedirectToAction("GetAll");
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(Place place)
        {
            await _genericService.Add(place);
            return Redirect("GetAll");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            return await GetUserFields(id);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id)
        {
            return await GetUserFields(id);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Place newPlace)
        {
            await _genericService.Update(newPlace);
            return RedirectToAction("GetAll");

        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int id)
        {
            return await GetUserFields(id);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Place place)
        {
            await _genericService.Remove(place);
            return RedirectToAction("GetAll");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var readResult = await _genericService.GetAll();
            return View(readResult);
        }

        [Authorize]
        public async Task<IActionResult> Sort(string searchString)
        {
            var places = await _genericService.GetAll();
            
            if (!String.IsNullOrEmpty(searchString.ToLower()))
            {
                places = places
                    .Where(s => s.Name.ToLower().Contains(searchString.ToLower()) 
                || s.OwnerShipType.ToLower().Contains(searchString.ToLower()) 
                || s.Phones.ToLower().Contains(searchString.ToLower()) 
                || s.Specialisation.ToLower().Contains(searchString.ToLower())
                || s.WorkTime.ToLower().Contains(searchString.ToLower()));
                
            }

            return View(places);
        }

        [HttpGet]
        [Authorize]
        private async Task<IActionResult> GetUserFields(int id)
        {
            var result = await _genericService.GetById(id);
            return View(result);
        }
    }
}
