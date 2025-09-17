using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Hospital.Services;
using Hospital.ViewModels;

namespace Hospital.Web.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    [Authorize(Roles = "Doctor")]
    public class DashboardController : Controller
    {
        private readonly IDoctorListService _doctorService;

        public DashboardController(IDoctorListService doctorService)
        {
            _doctorService = doctorService;
        }

        // GET: Doctor/Dashboard
        public IActionResult Index()
        {
            var doctors = _doctorService.GetAllDoctor(1, int.MaxValue); // Fetch all doctors without pagination
            return View(doctors.Data); // Send list to view
        }

        // GET: Doctor/Dashboard/Details/5
        public IActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var doctor = _doctorService.GetDoctorById(id);
            if (doctor == null) return NotFound();

            return View(doctor);
        }
    }
}
