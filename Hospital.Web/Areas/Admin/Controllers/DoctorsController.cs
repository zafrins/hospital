//using Hospital.Models;
//using Hospital.Services;
//using Hospital.ViewModels;
//using Microsoft.AspNetCore.Mvc;

//namespace Hospital.Web.Areas.Admin.Controllers
//{
//    public class DoctorsController : Controller
//    {
//        private readonly IApplicationUserService _userService;

//        public DoctorsController(IApplicationUserService userService)
//        {
//            _userService = userService;
//        }

//        // List doctors only
//        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
//        {
//            // Assuming you create a method to get only doctors
//            var doctors = _userService.GetAllDoctor(pageNumber, pageSize);
//            return View(doctors);
//        }

//        // Show form to create new doctor
//        public IActionResult Create()
//        {
//            return View(new ApplicationUserViewModel());
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Create(ApplicationUserViewModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                var user = model.ConvertViewModelToModel(new ApplicationUser());
//                user.IsDoctor = true;  // Mark as doctor
//                _userService.Create(user);
//                return RedirectToAction(nameof(Index));
//            }
//            return View(model);
//        }
//    }

//}
