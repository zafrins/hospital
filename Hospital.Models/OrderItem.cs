using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int MedicineId { get; set; }
        public string MedicineName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        // Foreign key
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}

