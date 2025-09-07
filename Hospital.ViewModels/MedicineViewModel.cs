using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Hospital.Models;

namespace Hospital.ViewModels
{
    public class MedicineViewModel
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        [Required, Range(0.01, double.MaxValue)]
        public decimal Cost { get; set; }

        public string Description { get; set; }

        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        public DateTime ExpiryDate { get; set; }
        public string Manufacturer { get; set; }

        // For file upload (not stored directly in DB)
        public IFormFile? ImageFile { get; set; }

        // Stored path of saved image
        public string? ImageUrl { get; set; }

        // ✅ Empty constructor
        public MedicineViewModel() { }

        // ✅ Constructor that maps from Medicine entity → ViewModel
        public MedicineViewModel(Medicine model)
        {
            Id = model.Id;
            Name = model.Name;
            Type = model.Type;
            Cost = model.Cost;
            Description = model.Description;
            Stock = model.Stock;
            ExpiryDate = model.ExpiryDate;
            Manufacturer = model.Manufacturer;
            ImageUrl = model.ImageUrl;
        }

        // ✅ Convert ViewModel → Medicine entity
        public Medicine ConvertViewModel(MedicineViewModel vm)
        {
            return new Medicine
            {
                Id = vm.Id,
                Name = vm.Name,
                Type = vm.Type,
                Cost = vm.Cost,
                Description = vm.Description,
                Stock = vm.Stock,
                ExpiryDate = vm.ExpiryDate,
                Manufacturer = vm.Manufacturer,
                ImageUrl = vm.ImageUrl // Will be set in Service after SaveImage()
            };
        }
    }
}
