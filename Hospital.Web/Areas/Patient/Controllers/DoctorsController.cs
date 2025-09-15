using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Hospital.Services;
using Hospital.ViewModels;

namespace Hospital.Web.Areas.Patient.Controllers
{
    [Area("Patient")]
    [Authorize(Roles = "Patient,Admin")]  // or just "Patient" if only patients view
    public class DoctorsController : Controller
    {
        private readonly IDoctorListService _doctorService;

        public DoctorsController(IDoctorListService doctorService)
        {
            _doctorService = doctorService;
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            var doctorsPaged = _doctorService.GetAllDoctor(pageNumber, pageSize);
            return View(doctorsPaged);
        }

        public IActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            var doctor = _doctorService.GetDoctorById(id);
            if (doctor == null) return NotFound();
            return View(doctor);
        }
    }
}
