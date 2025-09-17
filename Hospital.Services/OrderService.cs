using Hospital.Models;
using Hospital.Repositories;
using Hospital.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public int CreateOrder(string userId, List<CartItemViewModel> items)
        {
            if (items == null || items.Count == 0) throw new ArgumentException("Cart empty");

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                PaymentMethod = "Cash on Delivery",
            };

            decimal total = 0m;
            foreach (var it in items)
            {
                var oi = new OrderItem
                {
                    MedicineId = it.MedicineId,
                    MedicineName = it.MedicineName,
                    Price = it.Price,
                    Quantity = it.Quantity
                };
                total += oi.Price * oi.Quantity;
                order.Items.Add(oi);
            }

            order.TotalAmount = total;
            _context.Orders.Add(order);
            _context.SaveChanges();
            return order.Id;
        }

        public IEnumerable<Order> GetOrdersByUser(string userId)
        {
            return _context.Orders
                .Include(o => o.Items)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
        }

        public Order GetOrderById(int id)
        {
            return _context.Orders
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == id);
        }
        public IEnumerable<Order> GetAllOrders()
        {
            return _context.Orders
                .Include(o => o.Items)
                .OrderByDescending(o => o.OrderDate)
                .ToList();
        }

        public void UpdateOrderStatus(int orderId, string status)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);
            if (order != null)
            {
                order.Status = status;
                _context.Orders.Update(order);
                _context.SaveChanges();
            }
        }

    }
}
