using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Models
{
    public class Medicine
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Name { get; set; }

        [Required]
        public string Type { get; set; }

        [Required, Range(0.01, double.MaxValue)]
        public decimal Cost { get; set; }

        public string Description { get; set; }

        // ✅ New fields for Pharmacy Store
        public int Stock { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Manufacturer { get; set; }
        public string? ImageUrl { get; set; }

        public ICollection<MedicineReport> MedicineReport { get; set; }
        public ICollection<PrescribedMedicine> PrescribedMedicine { get; set; }
    }
}
