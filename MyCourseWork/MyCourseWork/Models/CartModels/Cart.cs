using System;
using System.Collections.Generic;
using System.Linq;
using Core.DbModels;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MyCourseWork.Models.CartModels
{
    public class Cart
    {
        private readonly HelperContext _helperContext;

        public Cart(HelperContext context)
        {
            _helperContext = context;
        }
        
        public string CartId { get; set; }
        
        public List<CartItem> CartItems { get; set; }

        public static Cart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            var context = services.GetService<HelperContext>();
            string cartId = session.GetString("cartId") ?? Guid.NewGuid().ToString();
            
            session.SetString("cartId", cartId);

            return new Cart(context) {CartId = cartId};
        }

        public void AddToCart(Product product, Cart cart)
        {
            var cartItem = _helperContext.CartItem.FirstOrDefault(x => x.Product.Id == product.Id && cart.CartId == x.CartProductId);
            if (cartItem != null)
            {
                cartItem.Quantity++;
                cartItem.Price += (float)product.Price;
                _helperContext.CartItem.Update(cartItem);
            }
            else
            {
                _helperContext.CartItem.Add(new CartItem()
                {
                    CartProductId = CartId,
                    Product = product,
                    Quantity = 1,
                    Price = (float)product.Price
                });
                
            }
            
            _helperContext.SaveChanges();
        }

        public void DeleteFromCart(int id)
        {
            var res = _helperContext.CartItem.FirstOrDefault(x => x.Product.Id == id && x.CartProductId == CartId);
            if (res.Quantity == 1)
            {
                _helperContext.CartItem.Remove(res);
            }
            else
            {
                res.Quantity--;
                res.Price -= (float)res.Product.Price;
                _helperContext.CartItem.Update(res);
            }

            _helperContext.SaveChanges();
        }

        public List<CartItem> GetItems()
        {
            return _helperContext.CartItem.Where(item => item.CartProductId == CartId).Include(s => s.Product).ToList();
        }
    }
}