using Hospital.Models;
using Hospital.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hospital.Web.Areas.Patient.Controllers
{
    [Authorize]
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
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // <-- Get logged-in user ID

                var appointment = new Appointment
                {
                    AppointmentDate = model.AppointmentDate,
                    CreatedDate = DateTime.Now,
                    Description = model.Reason,
                    DoctorId = model.DoctorId,
                    PatientId = userId,
                    Number = Guid.NewGuid().ToString().Substring(0, 8),
                    Type = "In-Person"
                };

                _context.Appointments.Add(appointment);
                _context.SaveChanges();

                return RedirectToAction("Confirmation");
            }

            return View(model);
        }
        
    public IActionResult MyAppointment()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var appointments = _context.Appointments
            .Where(a => a.PatientId == userId)
            .Include(a => a.Doctor)
            .OrderByDescending(a => a.AppointmentDate)
            .Select(a => new AppointmentViewModel
            {
                DoctorName = a.Doctor.Name ?? a.Doctor.UserName,
                AppointmentDate = a.AppointmentDate,
                Description = a.Description,
                AppointmentNumber = a.Number,
                Type = a.Type
            })
            .ToList();

        return View(appointments);
    }

    public IActionResult Confirmation()
        {
            return View();
        }


    }

}
