using Hospital.Models;
using Hospital.Repositories;
using Hospital.Repositories.Interfaces;
using Hospital.ViewModels;
using Hospitals.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Hospital.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _context;
        public PatientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public PagedResult<ApplicationUserViewModel> GetAll(int pageNumber, int pageSize)
        {
            int skip = (pageNumber - 1) * pageSize;
            var patients = _unitOfWork.Repository<ApplicationUser>()
                .GetAll(u => !u.IsDoctor)  // filter: only patients, no doctors
                .Skip(skip)
                .Take(pageSize)
                .ToList();

            int totalCount = _unitOfWork.Repository<ApplicationUser>()
                .GetAll(u => !u.IsDoctor)
                .Count();

            var patientViewModels = patients.Select(p => new ApplicationUserViewModel(p)).ToList();

            return new PagedResult<ApplicationUserViewModel>
            {
                Data = patientViewModels,
                TotalItems = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }



        private List<ApplicationUserViewModel> ConvertModelToViewModelList(List<ApplicationUser> patients)
        {
            throw new NotImplementedException();
        }

        public void Create(ApplicationUserViewModel model)
        {
            var user = new ApplicationUser
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name,
                Email = model.Email,
                UserName = model.UserName,
                Address = model.Address,
                Gender = model.Gender,
                IsDoctor = false,
                Specialist = null
            };

            _unitOfWork.Repository<ApplicationUser>().Add(user);
            _unitOfWork.Save();
        }


        /*public ApplicationUserViewModel GetById(string id)
        {
            // Get patient by ID logic
        }
*/

        public void Delete(string id)
        {
            var user = _unitOfWork.Repository<ApplicationUser>().GetById(id);
            if (user != null)
            {
                _unitOfWork.Repository<ApplicationUser>().Delete(user);
                _unitOfWork.Save(); // Make sure this commits changes
            }
        }



        public ApplicationUserViewModel GetById(string id)
        {
            var user = _unitOfWork.Repository<ApplicationUser>().GetById(id);

            if (user == null)
            {
                return null;
            }

            return new ApplicationUserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                UserName = user.UserName,
                Address = user.Address,
                Gender = user.Gender
                // Map other properties as necessary
            };
        }


        public void Update(ApplicationUserViewModel model)
        {
            var user = _unitOfWork.Repository<ApplicationUser>().GetById(model.Id);
            if (user == null) return;

            user.Name = model.Name;
            user.Email = model.Email;
            user.UserName = model.UserName;
            user.Address = model.Address;
            user.Gender = model.Gender;
            // Map other fields as needed

            _unitOfWork.Repository<ApplicationUser>().Add(user);
            _unitOfWork.Save();// Make sure this method commits changes
            
        }




    }

}
