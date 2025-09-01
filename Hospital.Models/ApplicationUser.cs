﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public Gender MyProperty{get;set;}

        public string Nationality { get; set; }

        public string Address { get; set; }
        public DateTime DOB { get; set; }
        public string Specialist { get; set; }

        public Department Department { get; set; }
        public ICollection<Appointment> Appointments { get; set; }

        public ICollection<Payroll> Payrolls { get; set; }
    }
}

namespace Hospital.Models
{
    public enum Gender
    {
        Male,Female,Other
    }
}

//eta ekta notun line code er branch test korar jonno
