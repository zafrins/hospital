using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Hospital.Services;
using Hospital.ViewModels;

namespace Hospital.Web.Areas.Doctor.Controllers
{
    [Area("Doctor")]
    [Authorize(Roles = "Doctor")]
    public class RoomsController : Controller
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        // GET: Doctor/RoomsForDoctor
        public IActionResult RoomsForDoctor()
        {
            var rooms = _roomService.GetAll(1, int.MaxValue); // fetch all rooms
            return View(rooms.Data);
        }
    }
}
