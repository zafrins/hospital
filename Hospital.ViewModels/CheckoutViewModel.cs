using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;

namespace Hospital.ViewModels
{
    public class CheckoutViewModel
    {
        public List<CartItemViewModel> Items { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Shipping { get; set; } = 0m; // none by default
        public decimal Total => Subtotal + Shipping;
    }
}

