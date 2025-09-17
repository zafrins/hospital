using Hospital.Models;
using Hospital.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Web.Areas.Patient.Controllers
{
    [Area("Patient")]
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Patient/Appointment/BookAppointment?doctorId=abc123
        public IActionResult BookAppointment(string doctorId)
        {
            if (string.IsNullOrEmpty(doctorId))
            {
                return BadRequest("Doctor ID is required.");
            }

            var doctor = _context.ApplicationUsers.FirstOrDefault(d => d.Id == doctorId);
            if (doctor == null)
            {
                return NotFound("Doctor not found.");
            }

            var model = new BookAppointmentViewModel
            {
                DoctorId = doctor.Id,
                DoctorName = doctor.Name ?? doctor.UserName // or adjust as needed
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult BookAppointment(BookAppointmentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Repopulate DoctorName if validation fails
                var doctor = _context.ApplicationUsers.FirstOrDefault(d => d.Id == model.DoctorId);
                model.DoctorName = doctor?.Name ?? doctor?.UserName ?? "Unknown";

                return View(model);
            }

            // Get currently logged-in user (the patient)
            var patientId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(patientId))
            {
                return Unauthorized("User must be logged in to book an appointment.");
            }

            // Create and save the appointment
            var appointment = new Appointment
            {
                DoctorId = model.DoctorId,
                PatientId = patientId,
                CreatedDate = DateTime.UtcNow,
                AppointmentDate = model.AppointmentDate,
                Description = model.Reason,
                Type = "General", // or set dynamically if needed
                // optional: auto-generated number
            };

            _context.Appointments.Add(appointment);
            _context.SaveChanges();

            return RedirectToAction("Confirmation"); // or wherever you want to go next
        }
        public IActionResult Confirmation()
        {
            return View();
        }


    }

}
