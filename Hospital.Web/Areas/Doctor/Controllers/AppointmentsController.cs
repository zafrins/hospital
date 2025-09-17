using Hospital.Models;
using Hospital.Repositories;
using Hospital.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Hospital.Web.Areas.Doctor.Controllers
{
    [Authorize(Roles = "Doctor")]
    [Area("Doctor")]
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Doctor/Appointments/Scheduled
        public IActionResult Scheduled()
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var appointments = _context.Appointments
                .Where(a => a.DoctorId == doctorId)
                .Include(a => a.Patient)
                .OrderByDescending(a => a.AppointmentDate)
                .Select(a => new DoctorAppointmentViewModel
                {
                    AppointmentId = a.Id,  // <-- ADD THIS
                    PatientName = a.Patient.Name ?? a.Patient.UserName,
                    PatientEmail = a.Patient.Email,
                    AppointmentDate = a.AppointmentDate,
                    Description = a.Description,
                    AppointmentNumber = a.Number,
                    Type = a.Type,
                    Status = a.Status
                })
                .ToList();

            return View(appointments);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Confirm(int id)
        {
            var doctorId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var appointment = _context.Appointments
                .FirstOrDefault(a => a.Id == id && a.DoctorId == doctorId);

            if (appointment == null)
            {
                return NotFound();
            }

            appointment.Status = "Confirmed";
            _context.SaveChanges();

            return RedirectToAction("Scheduled");
        }

    }
}
