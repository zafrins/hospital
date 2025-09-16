using Hospital.Services;
using Hospital.ViewModels;
using Hospital.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthorize]
    public class PatientsController : Controller
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            var patients = _patientService.GetAll(pageNumber, pageSize);
            return View(patients);
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            var patient = _patientService.GetById(id);
            if (patient == null) return NotFound();
            return View(patient);
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            var patient = _patientService.GetById(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }


        // POST: admin/Patient/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm); // Re-display form with validation errors
            }

            _patientService.Update(vm);
            return RedirectToAction("Index", "Patients"); // Redirect after save
        }

        // GET: admin/Patient/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: admin/Patient/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ApplicationUserViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _patientService.Create(vm);
                return RedirectToAction("Index", "Home"); // or redirect to a patient list
            }

            return View(vm);
        }

        // POST: /admin/Patients/Edit
        /* [HttpPost]
         public IActionResult Edit(ApplicationUserViewModel vm)
         {
             if (!ModelState.IsValid)
             {
                 return View(vm);
             }

             _patientService.Update(vm);  // This should update the entity and call Save() internally

             return RedirectToAction("Index");
         }*/

        /* [HttpPost]
         [ValidateAntiForgeryToken]
         public IActionResult Create(ApplicationUserViewModel model)
         {
             if (ModelState.IsValid)
             {
                 _patientService.CreatePatient(model);
                 return RedirectToAction("Index");
             }
             return View(model);
         }*/


  

        [HttpGet]
        public IActionResult Delete(string id)
        {
            var patient = _patientService.GetById(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient); // returns to Delete.cshtml
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(ApplicationUserViewModel model)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                return BadRequest("Invalid patient ID.");
            }

            _patientService.Delete(model.Id);
            return RedirectToAction("Index");
        }


        
    }
}
