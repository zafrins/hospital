using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class Order
    {
        public int Id { get; set; }

        // Identity user id (string)
        public string UserId { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        // "Cash on Delivery"
        public string PaymentMethod { get; set; } = "Cash on Delivery";

        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Pending";

        public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}

