using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MyCourseWork.Models;
using MyCourseWork.Models.CartModels;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace MyCourseWork.Repos
{
    public class OrderRepository : IOrdersRepository
    {
        private readonly HelperContext _context;

        private readonly Cart _cart;

        public OrderRepository(HelperContext context, Cart cart)
        {
            _context = context;
            _cart = cart;
        }

        public void CreateOrder(Order order)
        {
            order.OrderTime = DateTime.Now;

            _context.Orders.Add(order);
            
            _context.SaveChanges();

            var items = _cart.CartItems;
            
            foreach (var cartItem in items)
            {
                var detail = new OrderDetail()
                {
                    ProductId = cartItem.Product.Id,
                    OrderId = order.Id,
                    Price = cartItem.Price,
                    Quantity = cartItem.Quantity
                };
                
                _context.OrdersDetails.Add(detail);
            }

            _context.CartItem.RemoveRange(items);
            _context.BulkSaveChanges();
        }
    }
}