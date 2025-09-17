using Hospital.Models;
using Hospital.Repositories;
using Hospital.Services;
using Hospital.ViewModels;
using Hospitals.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Collections.Generic;
using Hospital.Web.Filters;

namespace Hospital.Web.Controllers
{
    public class PharmacyController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IOrderService _orderService;
        private readonly UserManager<IdentityUser> _userManager;
        private const string CART_SESSION_KEY = "Cart";

        public PharmacyController(ApplicationDbContext context, IOrderService orderService, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _orderService = orderService;
            _userManager = userManager;
        }


        [AllowAnonymous]
        public IActionResult Index()
        {
            var medicines = _context.Medicines.ToList();
            return View(medicines);
        }

        // Add to cart — requires login to add (per your requirement)
        [HttpPost]
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            if (!User.Identity.IsAuthenticated)
            {
                // redirect to login, after login return to store
                return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = Url.Action("Index", "Pharmacy") });
            }

            var med = _context.Medicines.Find(id);
            if (med == null) return NotFound();

            // Load cart from session
            var cart = HttpContext.Session.GetObject<List<CartItemViewModel>>(CART_SESSION_KEY) ?? new List<CartItemViewModel>();

            var existing = cart.FirstOrDefault(c => c.MedicineId == id);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartItemViewModel
                {
                    MedicineId = med.Id,
                    MedicineName = med.Name,
                    Price = med.Cost,
                    Quantity = quantity,
                    ImageUrl = med.ImageUrl
                });
            }

            HttpContext.Session.SetObject(CART_SESSION_KEY, cart);

            return RedirectToAction("Cart");
        }

        // Cart page - requires login (only logged-in can checkout)
        public IActionResult Cart()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = Url.Action("Cart", "Pharmacy") });
            }

            var cart = HttpContext.Session.GetObject<List<CartItemViewModel>>(CART_SESSION_KEY) ?? new List<CartItemViewModel>();
            return View(cart);
        }

        // Update cart quantities (POST)
        [HttpPost]
        public IActionResult UpdateCart(List<CartItemViewModel> items)
        {
            HttpContext.Session.SetObject(CART_SESSION_KEY, items ?? new List<CartItemViewModel>());
            return RedirectToAction("Cart");
        }

        // Remove one item
        [HttpPost]
        public IActionResult RemoveFromCart(int medicineId)
        {
            var cart = HttpContext.Session.GetObject<List<CartItemViewModel>>(CART_SESSION_KEY) ?? new List<CartItemViewModel>();
            var existing = cart.FirstOrDefault(c => c.MedicineId == medicineId);
            if (existing != null)
            {
                cart.Remove(existing);
                HttpContext.Session.SetObject(CART_SESSION_KEY, cart);
            }
            return RedirectToAction("Cart");
        }

        // Checkout page
        public IActionResult Checkout()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = Url.Action("Checkout", "Pharmacy") });
            }
            var cart = HttpContext.Session.GetObject<List<CartItemViewModel>>(CART_SESSION_KEY) ?? new List<CartItemViewModel>();
            var vm = new CheckoutViewModel
            {
                Items = cart,
                Subtotal = cart.Sum(i => i.Total),
                Shipping = 0m
            };
            return View(vm);
        }

        // Confirm order (POST)
        [HttpPost]
        public IActionResult ConfirmOrder()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = Url.Action("Checkout", "Pharmacy") });
            }

            var cart = HttpContext.Session.GetObject<List<CartItemViewModel>>(CART_SESSION_KEY) ?? new List<CartItemViewModel>();
            if (cart.Count == 0) return RedirectToAction("Cart");

            var userId = _userManager.GetUserId(User);
            var orderId = _orderService.CreateOrder(userId, cart);

            // clear cart
            HttpContext.Session.Remove(CART_SESSION_KEY);

            return RedirectToAction("OrderConfirmed", new { id = orderId });
        }

        public IActionResult OrderConfirmed(int id)
        {
            var order = _orderService.GetOrderById(id);
            if (order == null) return NotFound();
            return View(order);
        }

        // My orders page — show logged-in user's orders
        public IActionResult MyOrders()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity", returnUrl = Url.Action("MyOrders", "Pharmacy") });
            }
            var userId = _userManager.GetUserId(User);
            var orders = _orderService.GetOrdersByUser(userId);
            return View(orders);
        }
        [AdminAuthorize]
        public IActionResult ManageOrders()
        {
            var orders = _orderService.GetAllOrders();
            return View(orders);
        }

        [AdminAuthorize]
        [HttpPost]
        public IActionResult UpdateStatus(int id, string status)
        {
            _orderService.UpdateOrderStatus(id, status);
            return RedirectToAction("ManageOrders");
        }

    }
}
