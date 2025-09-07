using Hospital.Models;
using Hospital.Repositories.Interfaces;
using Hospital.ViewModels;
using Hospitals.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hospital.Services
{
    public class MedicineService : IMedicineService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _env;

        public MedicineService(IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            _unitOfWork = unitOfWork;
            _env = env;
        }

        public PagedResult<MedicineViewModel> GetAll(int pageNumber, int pageSize)
        {
            int totalCount;
            List<MedicineViewModel> vmList = new List<MedicineViewModel>();

            try
            {
                int ExcludeRecords = (pageSize * pageNumber) - pageSize;

                var modelList = _unitOfWork.Repository<Medicine>()
                    .GetAll()
                    .Skip(ExcludeRecords)
                    .Take(pageSize)
                    .ToList();

                totalCount = _unitOfWork.Repository<Medicine>().GetAll().Count();

                vmList = ConvertToViewModel(modelList);
            }
            catch (Exception)
            {
                throw;
            }

            return new PagedResult<MedicineViewModel>
            {
                Data = vmList,
                TotalItems = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public MedicineViewModel GetMedicineById(int id)
        {
            var model = _unitOfWork.Repository<Medicine>().GetById(id);
            if (model == null) return null;

            return new MedicineViewModel(model);
        }

        public void InsertMedicine(MedicineViewModel vm)
        {
            var model = new MedicineViewModel().ConvertViewModel(vm);
            model.ImageUrl = SaveImage(vm.ImageFile);

            _unitOfWork.Repository<Medicine>().Add(model);
            _unitOfWork.Save();
        }

        public void UpdateMedicine(MedicineViewModel vm)
        {
            var model = _unitOfWork.Repository<Medicine>().GetById(vm.Id);
            if (model == null) return;

            model.Name = vm.Name;
            model.Type = vm.Type;
            model.Cost = vm.Cost;
            model.Description = vm.Description;
            model.Stock = vm.Stock;
            model.ExpiryDate = vm.ExpiryDate;
            model.Manufacturer = vm.Manufacturer;

            if (vm.ImageFile != null)
            {
                model.ImageUrl = SaveImage(vm.ImageFile);
            }

            _unitOfWork.Repository<Medicine>().Update(model);
            _unitOfWork.Save();
        }

        public void DeleteMedicine(int id)
        {
            var model = _unitOfWork.Repository<Medicine>().GetById(id);
            if (model == null) return;

            _unitOfWork.Repository<Medicine>().Delete(model);
            _unitOfWork.Save();
        }

        public List<MedicineViewModel> ConvertToViewModel(List<Medicine> modelList)
        {
            return modelList.Select(x => new MedicineViewModel(x)).ToList();
        }

        private string? SaveImage(IFormFile? file)
        {
            if (file == null) return null;

            string uploadsFolder = Path.Combine(_env.WebRootPath, "images", "medicines");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            string filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return "/images/medicines/" + fileName;
        }
    }
}
