using cloudscribe.Pagination.Models;
using Hospital.Models;
using Hospital.Services;
using Hospital.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Hospital.Web.Controllers
{
    [Area("admin")]
    public class ContactsController : Controller
    {

        private IRoomService _contact;
        private IHospitalInfo _hospitalInfo;

        public ContactsController(IContactService contact,IHospitalInfo hospitalInfo)
        {
            _contact = contact;
            _hospitalInfo = hospitalInfo;
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            return View(_contact.GetAll(pageNumber, pageSize));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            ViewBag.Hospitals = new SelectList(_hospitalInfo.GetAll(1, 100).Data, "Id", "Name");
            var viewModel = _contact.GetContactByID(id);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Edit(RoomViewModel vm)
        {

            _room.UpdateRoom(vm);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(RoomViewModel vm)
        {
            _room.InsertRoom(vm);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            _room.DeleteRoom(id);
            return View("Index");
        }
    }
}
