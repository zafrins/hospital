using Hospital.Models;
using Microsoft.AspNetCore.Mvc.Rendering; // for SelectListItem
using System;
using System.Collections.Generic;

namespace Hospital.ViewModels
{
    public class TimingViewModel
    {
        public int Id { get; set; }

        public ApplicationUser Doctor { get; set; }

        public DateTime Date { get; set; }

        public int MorningShiftStartTime { get; set; }
        public int MorningShiftEndTime { get; set; }
        public int AfternoonShiftStartTime { get; set; }
        public int AfternoonShiftEndTime { get; set; }
        public int Duration { get; set; } // in minutes

        public Status Status { get; set; }

        // Shift dropdown lists
        public List<SelectListItem> MorningShiftStartOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> MorningShiftEndOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AfternoonShiftStartOptions { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AfternoonShiftEndOptions { get; set; } = new List<SelectListItem>();

        public ApplicationUser DoctorId { get; set; }
        public TimingViewModel() { }

        public TimingViewModel(Timing model)
        {
            Id = model.Id;
            Date = model.Date;
            MorningShiftStartTime = model.MorningShiftStartTime;
            MorningShiftEndTime = model.MorningShiftEndTime;
            AfternoonShiftStartTime = model.AfternoonShiftStartTime;
            AfternoonShiftEndTime = model.AfternoonShiftEndTime;
            Duration = model.Duration;
            Status = model.Status;
            DoctorId = model.DoctorId;
        }

        public Timing ConvertViewModel(TimingViewModel model)
        {
            return new Timing
            {
                Id = model.Id,
                Date = model.Date,
                MorningShiftStartTime = model.MorningShiftStartTime,
                MorningShiftEndTime = model.MorningShiftEndTime,
                AfternoonShiftStartTime = model.AfternoonShiftStartTime,
                AfternoonShiftEndTime = model.AfternoonShiftEndTime,
                Duration = model.Duration,
                Status = model.Status,
                DoctorId = model.DoctorId,
            };
        }
    }
}
