using Hospital.Models;
using Hospital.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Services
{
    public interface IOrderService
    {
        int CreateOrder(string userId, List<CartItemViewModel> items);
        IEnumerable<Hospital.Models.Order> GetOrdersByUser(string userId);
        Hospital.Models.Order GetOrderById(int id);
        IEnumerable<Order> GetAllOrders();
        void UpdateOrderStatus(int orderId, string status);

    }

}
