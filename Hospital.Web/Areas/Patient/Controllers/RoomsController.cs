using Hospital.Services;
using Hospital.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Hospital.Web.Areas.Patient.Controllers
{
    [Area("Patient")]
    [Authorize(Roles = "Patient")]
    public class RoomsController : Controller
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        // GET: /Patient/Rooms
        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            var roomsPaged = _roomService.GetAll(pageNumber, pageSize);
            return View(roomsPaged);
        }

        // Optional: Details page for a room
        public IActionResult Details(int id)
        {
            var room = _roomService.GetRoomById(id);
            if (room == null) return NotFound();
            return View(room);
        }
    }
}
