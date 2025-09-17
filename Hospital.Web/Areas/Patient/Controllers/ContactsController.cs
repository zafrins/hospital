using Hospital.Services;
using Hospital.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Hospitals.Utilities;

namespace Hospital.Web.Controllers
{
    [Area("Patient")]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;

        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        // GET: /Contact
        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            var contactsPaged = _contactService.GetAll(pageNumber, pageSize);
            return View(contactsPaged);
        }

        // Optional: Contact details page
        public IActionResult Details(int id)
        {
            var contact = _contactService.GetContactById(id);
            if (contact == null) return NotFound();
            return View(contact);
        }
    }
}
