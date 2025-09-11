using Hospital.Models;
using Hospital.Repositories.Interfaces;
using Hospital.ViewModels;
using Hospitals.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Hospital.Services
{
    public class ApplicationUserService : IApplicationUserService
    {
        private IUnitOfWork _unitOfWork;

        public ApplicationUserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void CreatePatient(ApplicationUserViewModel model)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }


        public PagedResult<ApplicationUserViewModel> GetAll(int pageNumber, int pageSize)
        {
            try
            {
                // Your existing logic here
                int skip = (pageNumber - 1) * pageSize;

                var patients = _unitOfWork.Repository<ApplicationUser>()
                    .GetAll(u => !u.IsDoctor)
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                var totalCount = _unitOfWork.Repository<ApplicationUser>()
                    .GetAll(u => !u.IsDoctor)
                    .Count();

                var vmList = ConvertModelToViewModelList(patients);

                return new PagedResult<ApplicationUserViewModel>
                {
                    Data = vmList,
                    TotalItems = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                };
            }
            catch (Exception)
            {
                // Handle or rethrow exception if necessary
                throw;
            }
        }


        public PagedResult<ApplicationUserViewModel> GetAllDoctor(int PageNumber, int PageSize)
         {
            var vm = new ApplicationUserViewModel();
            int totalCount;
            List<ApplicationUserViewModel> vmList = new List<ApplicationUserViewModel>();
            try
            {
                int ExcludeRecords = (PageSize * PageNumber) - PageSize;

                var modelList = _unitOfWork.Repository<ApplicationUser>()
                      .GetAll(x=>x.IsDoctor==true).Skip(ExcludeRecords).Take(PageSize).ToList();

                totalCount = _unitOfWork.Repository<ApplicationUser>().GetAll(x=>x.IsDoctor==true).ToList().Count;

                vmList = ConvertModelToViewModelList(modelList);

            }
            catch (Exception)
            {
                throw;
            }

            var result = new PagedResult<ApplicationUserViewModel>
            {

                Data = vmList,
                TotalItems = totalCount,
                PageNumber = PageNumber,
                PageSize = PageSize



            };
            return result;
        }

        /*
         *  public PagedResult<ApplicationUserViewModel> GetAll(int PageNumber, int PageSize)
        public PagedResult<ApplicationUserViewModel> GetAllDoctor(int PageNumber, int PageSize)
        {
            int totalCount;
            List<ApplicationUserViewModel> vmList = new List<ApplicationUserViewModel>();
            try
            {
                int skip = (PageNumber - 1) * PageSize;
                var modelList = _unitOfWork.Repository<ApplicationUser>()
                    .GetAll()
                    .Where(u => u.IsDoctor)
                    .Skip(skip)
                    .Take(PageSize)
                    .ToList();

                totalCount = _unitOfWork.Repository<ApplicationUser>()
                    .GetAll()
                    .Count(u => u.IsDoctor);

                vmList = ConvertModelToViewModelList(modelList);
            }
            catch (Exception)
            {
                throw;
            }
            return new PagedResult<ApplicationUserViewModel>
            {
                Data = vmList,
                TotalItems = totalCount,
                PageNumber = PageNumber,
                PageSize = PageSize
            };
        }
        */
        public PagedResult<ApplicationUserViewModel> GetAllPatient(int PageNumber, int PageSize)
        {
            int totalCount;
            List<ApplicationUserViewModel> vmList = new List<ApplicationUserViewModel>();
            try
            {
                int skip = (PageNumber - 1) * PageSize;
                var modelList = _unitOfWork.Repository<ApplicationUser>()
                    .GetAll()
                    .Where(u => !u.IsDoctor)
                    .Skip(skip)
                    .Take(PageSize)
                    .ToList();

                totalCount = _unitOfWork.Repository<ApplicationUser>()
                    .GetAll()
                    .Count(u => !u.IsDoctor);

                vmList = ConvertModelToViewModelList(modelList);
            }
            catch (Exception)
            {
                throw;
            }
            return new PagedResult<ApplicationUserViewModel>
            {
                Data = vmList,
                TotalItems = totalCount,
                PageNumber = PageNumber,
                PageSize = PageSize
            };
        }

        public ApplicationUserViewModel GetById(string id)
        {
            throw new NotImplementedException();
        }

        /*public PagedResult<ApplicationUserViewModel> GetAllPatient(int PageNumber, int PageSize)
        {
            throw new NotImplementedException();
        }*/

        public PagedResult<ApplicationUserViewModel> SearchDoctor(int PageNumber, int PageSize, string Speciality = null)
        {
            throw new NotImplementedException();
        }

        public void UpdatePatient(ApplicationUserViewModel model)
        {
            throw new NotImplementedException();
        }

        private List<ApplicationUserViewModel> ConvertModelToViewModelList(List<ApplicationUser> modelList)
        {
            return modelList.Select(x => new ApplicationUserViewModel(x)).ToList();
        }
    }
}

