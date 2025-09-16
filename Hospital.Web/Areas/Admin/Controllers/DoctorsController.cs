using Hospital.Services;
using Hospital.ViewModels;
using Hospital.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthorize]
    public class DoctorTimingsController : Controller
    {
        private readonly IDoctorService _doctorService;

        public DoctorTimingsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        // List all doctor timings
        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            var timings = _doctorService.GetAll(pageNumber, pageSize);
            return View("~/Areas/Admin/Views/Doctor/Index.cshtml", timings);
        }

        // Show create form
        public IActionResult Create()
        {
            return View("~/Areas/Admin/Views/Doctor/Create.cshtml", new TimingViewModel());

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TimingViewModel model)
        {
            if (ModelState.IsValid)
            {
                _doctorService.AddTiming(model);
                return RedirectToAction(nameof(Index));
            }
            return View("~/Areas/Admin/Views/Doctor/Create.cshtml", model);

        }

        // Show edit form
        public IActionResult Edit(int id)
        {
            var timing = _doctorService.GetTimingById(id);
            if (timing == null)
                return NotFound();

            return View("~/Areas/Admin/Views/Doctor/Edit.cshtml", timing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TimingViewModel model)
        {
            if (ModelState.IsValid)
            {
                _doctorService.UpdateTiming(model);
                return RedirectToAction(nameof(Index));
            }
            return View("~/Areas/Admin/Views/Doctor/Edit.cshtml", model);
        }


        // Delete action
        public IActionResult Delete(int id)
        {
            var timing = _doctorService.GetTimingById(id);
            if (timing == null)
                return NotFound();

            _doctorService.DeleteTiming(id);
            return RedirectToAction(nameof(Index));
        }

        //details
       /* [HttpGet]
        public IActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            var patient = _doctorService.GetById(id);
            if (patient == null) return NotFound();
            return View(patient);
        }*/
    }
}


//ager kora code, kintu khule dekhi comment out, rekhe dilam emnei


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
//                _userService.Create(user); //error on _userService
//                return RedirectToAction(nameof(Index));
//            }
//            return View(model);
//        }
//    }

//}
