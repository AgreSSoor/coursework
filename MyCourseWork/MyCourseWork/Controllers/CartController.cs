using System.Linq;
using System.Threading.Tasks;
using Core.DbModels;
using DALAbstractions.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyCourseWork.Models;
using MyCourseWork.Models.CartModels;

namespace MyCourseWork.Controllers
{
    public class CartController : Controller
    {
        private readonly IGenericRepository<Product> _repository;
        
        private readonly IGenericRepository<CartItem> _cartItemRepository;
        
        private readonly Cart _cart;

        public CartController(IGenericRepository<Product> repository, Cart cart, IGenericRepository<CartItem> cartItemRepository)
        {
            _repository = repository;
            _cart = cart;
            _cartItemRepository = cartItemRepository;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var items = await _cartItemRepository.GetWhere(x => x.CartProductId == _cart.CartId);
            var product = await _repository.GetAll();
            _cart.CartItems = items.ToList();

            if (_cart.CartItems.Count == 0)
            {
                ModelState.AddModelError("", "Корзина пуста");
                ViewBag.Message = "Корзина пуста";
                return View();
            }

            var obj = new CartViewModel()
            {
                Cart = _cart
            };

            return View(obj);
        }

        [Authorize]
        public RedirectToActionResult AddToCart(int id)
        {
            var item = _repository.GetAll().Result.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                _cart.AddToCart(item, _cart);
            }

            return RedirectToAction(nameof(Index), "Product");
        }
        
        [Authorize]
        public RedirectToActionResult RemoveFromCart(int id)
        {
            var item = _repository.GetAll().Result.FirstOrDefault(x => x.Id == id);
            if (item != null)
            {
                _cart.DeleteFromCart(id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}