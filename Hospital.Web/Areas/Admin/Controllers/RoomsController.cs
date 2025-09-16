using cloudscribe.Pagination.Models;
using Hospital.Models;
using Hospital.Services;
using Hospital.ViewModels;
using Hospital.Web.Filters;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Web.Controllers
{
    [Area("Admin")]
    [AdminAuthorize]
    public class RoomsController : Controller
    {

        private IRoomService _room;
        private IHospitalInfo _hospitalInfo;

        public RoomsController(IRoomService room, IHospitalInfo hospitalInfo)
        {
            _room = room;
            _hospitalInfo = hospitalInfo;
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            return View(_room.GetAll(pageNumber, pageSize));
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var viewModel = _room.GetRoomById(id);
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
