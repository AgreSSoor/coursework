using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLLAbstractions.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Core.DbModels;
using Microsoft.AspNetCore.Authorization;
using MyCourseWork.Models;

namespace MyCourseWork.Controllers
{
    public class ProductController : Controller
    {
        private readonly IGenericService<Product> _genericService;

        private readonly IGenericService<Place> _placeService;

        public ProductController(HelperContext context, IGenericService<Product> service,
            IGenericService<Place> placeService)
        {
            _genericService = service;
            _placeService = placeService;
        }

        // GET: Product
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var res = await _genericService.GetAll();
            await _placeService.GetAll();
            Dictionary<Product, int> dict = new Dictionary<Product, int>();
            foreach (var product in res)
            {
                dict.Add(product, 0); 
            }
            ViewData["Quantity"] = dict;
            
            
            
            return View(res);
        }

        // GET: Product/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _genericService.GetAll();

            await _placeService.GetAll();
            
            var product = products
                .FirstOrDefault(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Product/Create
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create()
        {
            ViewData["PlaceId"] = new SelectList(await _placeService.GetAll(), "Id", "Name");
            return View();
        }

        // POST: Product/Create
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Price,Description,PlaceId,Id")] Product product)
        {
            if (ModelState.IsValid)
            {
                //_context.Add(product);
                await _genericService.Add(product);
                //await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlaceId"] = new SelectList(await _placeService.GetAll(), "Id", "Name", product.PlaceId);
            return View(product);
        }

        // GET: Product/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _genericService.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["PlaceId"] = new SelectList(await _placeService.GetAll(), "Id", "Name", product.PlaceId);
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Price,Description,PlaceId,Id")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _genericService.Update(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PlaceId"] = new SelectList(await _placeService.GetAll(), "Id", "Name", product.PlaceId);
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = await _genericService.GetAll();
                    
            await _placeService.GetAll();

            var product = products
                .FirstOrDefault(m => m.Id == id);
            
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _genericService.FirstOrDefault(x => x.Id == id);
            await _genericService.Remove(product);
            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> Sort(string searchString)
        {
            var products = await _genericService.GetAll();
            await _placeService.GetAll();
            if (!String.IsNullOrEmpty(searchString.ToLower()))
            {
                products =  products
                    .Where(s => s.Name.ToLower().Contains(searchString.ToLower())
                                || s.Price.ToString().ToLower().Contains(searchString.ToLower()) 
                                || s.Place.Name.ToLower().Contains(searchString.ToLower())
                                || s.Description.ToLower().Contains(searchString.ToLower()));
                
            }

            return View(products);
        }
        
        private async Task<bool> ProductExists(int id)
        {
            var res = await _genericService.GetAll();
            return res.Any(e => e.Id == id);
        }
    }
}
