using cloudscribe.Pagination.Models;
using Hospital.Services;
using Hospital.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Hospital.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MedicinesController : Controller
    {
        private readonly IMedicineService _medicine;

        public MedicinesController(IMedicineService medicine)
        {
            _medicine = medicine;
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = 10)
        {
            return View(_medicine.GetAll(pageNumber, pageSize));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(MedicineViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _medicine.InsertMedicine(vm);
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var vm = _medicine.GetMedicineById(id);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Edit(MedicineViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _medicine.UpdateMedicine(vm);
                return RedirectToAction("Index");
            }
            return View(vm);
        }

        public IActionResult Delete(int id)
        {
            _medicine.DeleteMedicine(id);
            return RedirectToAction("Index");
        }
    }
}
