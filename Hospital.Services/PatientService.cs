using Hospital.Models;
using Hospital.Repositories.Interfaces;
using Hospital.ViewModels;
using Hospitals.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Hospital.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;

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
            // Add new patient logic
        }

        /*public ApplicationUserViewModel GetById(string id)
        {
            // Get patient by ID logic
        }
*/
        public void Update(ApplicationUserViewModel model)
        {
            // Update patient logic
        }

        public void Delete(string id)
        {
            // Delete patient logic
        }

        public ApplicationUserViewModel GetById(string id)
        {
            throw new NotImplementedException();
        }
    }

}
