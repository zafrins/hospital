using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Hospital.Services;
using Hospital.ViewModels;

namespace Hospital.Web.Areas.Patient.Controllers
{
    [Area("Patient")]
    [Authorize(Roles = "Patient")]
    public class PatientsController : Controller
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        // GET: /Patient/Patients/Index
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }
            var patient = _patientService.GetById(userId);
            if (patient == null) return NotFound();
            return View("Details", patient);  // or a custom view if you want
        }

        // GET: /Patient/Patients/Details
        public IActionResult Details()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var patient = _patientService.GetById(userId);
            if (patient == null) return NotFound();

            return View(patient);
        }

        // GET: /Patient/Patients/Edit
        [HttpGet]
        public IActionResult Edit()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var patient = _patientService.GetById(userId);
            if (patient == null) return NotFound();

            return View(patient);
        }

        // POST: /Patient/Patients/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationUserViewModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            if (model.Id != userId)
                return Forbid();

            if (!ModelState.IsValid)
                return View(model);

            _patientService.Update(model);
            return RedirectToAction(nameof(Details));
        }

        // GET: /Patient/Patients/Delete
        [HttpGet]
        public IActionResult Delete()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var patient = _patientService.GetById(userId);
            if (patient == null) return NotFound();

            return View(patient);
        }

        // POST: /Patient/Patients/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            _patientService.Delete(userId);
            // optionally log out after deletion
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
