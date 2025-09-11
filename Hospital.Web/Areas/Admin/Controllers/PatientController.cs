using Hospital.Services;
using Hospital.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PatientsController : Controller
    {
        private readonly IApplicationUserService _patientService;

        public PatientsController(IApplicationUserService patientService)
        {
            _patientService = patientService;
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            var result = _patientService.GetAllPatient(pageNumber, pageSize);
            return View(result.Data);  // Passing IEnumerable<ApplicationUserViewModel>
        }
    }
}
