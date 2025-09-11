using Hospital.Services;
using Hospital.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Web.Areas.Admin.Controllers
{
    [Area("admin")]
    public class DoctorListController : Controller
    {
        private readonly IDoctorListService _doctorService;

        public DoctorListController(IDoctorListService doctorService)
        {
            _doctorService = doctorService;
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            var doctors = _doctorService.GetAllDoctor(pageNumber, pageSize);
            return View(doctors); // View: Views/DoctorList/Index.cshtml
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            var doctor = _doctorService.GetDoctorById(id);
            if (doctor == null) return NotFound();
            return View(doctor);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateDoctor(ApplicationUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _doctorService.CreateDoctor(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var doctor = _doctorService.GetDoctorById(id);
            if (doctor == null) return NotFound();
            return View(doctor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationUserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            _doctorService.UpdateDoctor(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeleteDoctor(string id)
        {
            var doctor = _doctorService.GetDoctorById(id);
            if (doctor == null) return NotFound();
            return View(doctor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(ApplicationUserViewModel model)
        {
            if (string.IsNullOrEmpty(model.Id)) return BadRequest();
            _doctorService.DeleteDoctor(model.Id);
            return RedirectToAction("Index");
        }
    }
}
