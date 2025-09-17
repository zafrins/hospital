using Hospital.Models;
using Hospital.Services;
using Hospital.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        // Convenience property for current user id
        private string? CurrentUserId =>
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        public IActionResult Index()
        {
            var patient = GetLoggedInPatient();
            if (patient == null) return Unauthorized();

            return View("Details", patient);
        }

        public IActionResult Details()
        {
            var patient = GetLoggedInPatient();
            if (patient == null) return Unauthorized();

            return View(patient);
        }

        [HttpGet]
        public IActionResult Edit()
        {
            var patient = GetLoggedInPatient();
            if (patient == null) return Unauthorized();

            return View(patient);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationUserViewModel model)
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            if (model.Id != userId)
                return Forbid();

            if (!ModelState.IsValid)
                return View(model);

            _patientService.Update(model);
            return RedirectToAction(nameof(Details));
        }

        [HttpGet]
        public IActionResult Delete()
        {
            var patient = GetLoggedInPatient();
            if (patient == null) return Unauthorized();

            return View(patient);
        }

        // POST: /Patient/Patients/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed()
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            _patientService.Delete(userId);


            return RedirectToAction("Index", "Home", new { area = "" });
        }

        private ApplicationUserViewModel? GetLoggedInPatient()
        {
            var userId = CurrentUserId;
            if (string.IsNullOrEmpty(userId))
                return null;

            return _patientService.GetById(userId);
        }
    }
}
