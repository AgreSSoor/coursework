using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCourseWork.Models;
using MyCourseWork.Models.CartModels;
using MyCourseWork.Repos;

namespace MyCourseWork.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrdersRepository _ordersRepository;

        private readonly Cart _cart;
        
        public OrderController(IOrdersRepository context, Cart cart)
        {
            _ordersRepository = context;
            _cart = cart;
        }

        
        // GET
        [Authorize]
        public IActionResult Checkout()
        {
            return View();
        }
        
        [HttpPost]
        [Authorize]
        public IActionResult Checkout(Order order)
        {
            _cart.CartItems  = _cart.GetItems();

            if (ModelState.IsValid)
            {
                _ordersRepository.CreateOrder(order);
                return RedirectToAction(nameof(Complete));
                
            }
            
            return View(order);
        }

        [Authorize]
        public IActionResult Complete()
        {
            ViewBag.Message = "Заказ ушёл на обработку";
            return View();
        }
    }
}