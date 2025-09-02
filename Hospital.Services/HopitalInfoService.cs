using Hospital.Models;
using Hospital.Repositories.Interfaces;
using Hospital.ViewModels;
using Hospitals.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Services
{
    public class HopitalInfoService : IHospitalInfo
    {
        private IUnitOfWork _unitOfWork;
        public HopitalInfoService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void DeleteHospitalInfo(int id)
        {
            var model = _unitOfWork.Repository<HospitalInfo>().GetById(id);
            _unitOfWork.Repository<HospitalInfo>().Delete(model);
            _unitOfWork.Save();
        }

        public PagedResult<HospitalInfoViewModel> GetAll(int pageNumber, int pageSize)
        {
            int totalCount = 0;
            List<HospitalInfoViewModel> vmList = new List<HospitalInfoViewModel>();
            try
            {
                int ExcludeRecords = (pageSize * pageNumber) - pageSize;

                var modelList = _unitOfWork.Repository<HospitalInfo>()
                    .GetAll()
                    .Skip(ExcludeRecords)
                    .Take(pageSize)
                    .ToList();

                totalCount = _unitOfWork.Repository<HospitalInfo>().GetAll().Count();

                foreach (var item in modelList)
                {
                    vmList.Add(new HospitalInfoViewModel
                    {
                        Id = item.Id,
                        Name = item.Name,
                        Type = item.Type,
                        City = item.City,
                        PinCode = item.PinCode,
                        Country = item.Country
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }

            var result = new PagedResult<HospitalInfoViewModel>
            {
                Data = vmList,
                TotalItems = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return result;
        }

        public HospitalInfoViewModel GetHospitalById(int HospitalId)
        {
            var model = _unitOfWork.Repository<HospitalInfo>().GetById(HospitalId);
            var vm = new HospitalInfoViewModel(model);
            return vm;
        }

        public void InsertHospitalInfo(HospitalInfoViewModel hospitalInfo)
        {
            var model = new HospitalInfoViewModel().ConvertViewModel(hospitalInfo);
            _unitOfWork.Repository<HospitalInfo>().Add(model);
            _unitOfWork.Save();
        }

        public void UpdateHospitalInfo(HospitalInfoViewModel hospitalInfo)
        {
            var model = new HospitalInfoViewModel().ConvertViewModel(hospitalInfo);
            var ModelById = _unitOfWork.Repository<HospitalInfo>().GetById(model.Id);
            ModelById.Name = hospitalInfo.Name;
            ModelById.Type = hospitalInfo.Type;
            ModelById.City = hospitalInfo.City;
            ModelById.PinCode = hospitalInfo.PinCode;
            ModelById.Country = hospitalInfo.Country;
            _unitOfWork.Repository<HospitalInfo>().Update(ModelById);
            _unitOfWork.Save();
        }

        private List<HospitalInfoViewModel> ConvertToViewModel(List<HospitalInfo> modelList)
        {
            return modelList.Select(x => new HospitalInfoViewModel(x)).ToList();

        }
    }
}
