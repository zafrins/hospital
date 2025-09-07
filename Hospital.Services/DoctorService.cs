using Hospital.Models;
using Hospital.Repositories.Implementation;
using Hospital.Repositories.Interfaces;
using Hospital.ViewModels;
using Hospitals.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital.Services
{
    public class DoctorService : IDoctorService
    {
        private IUnitOfWork _unitOfWork;

        public DoctorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void AddTiming(TimingViewModel timing)
        {
            var model = timing.ConvertViewModel(timing); // returns Timing
            _unitOfWork.Repository<Timing>().Add(model); // use Timing, not TimingViewModel
                                                         //I got an error here before changed some stuffs
            _unitOfWork.Save();
        }
        public void DeleteTiming(int TimingId)
        {
            var model = _unitOfWork.Repository<TimingViewModel>().GetById(TimingId);
            _unitOfWork.Repository<TimingViewModel>().Delete(model);
            _unitOfWork.Save();
        }

        public PagedResult<TimingViewModel> GetAll(int pageNumber, int pageSize) //error here on GetAll
        {
            var vm = new TimingViewModel();
            int totalcount;
            List<TimingViewModel> vmList = new List<TimingViewModel>();
            try
            {
                int ExcludeRecords = (pageSize * pageNumber) - pageSize;

                var modelList = _unitOfWork.Repository<Timing>()
                    .GetAll()
                    .Skip(ExcludeRecords)
                    .Take(pageSize)
                    .ToList();

                totalcount = _unitOfWork.Repository<Timing>().GetAll().Count();

                vmList = ConvertModelToViewModelList(modelList);
                //was an error here, changed stuffs

            }
            catch (Exception)
            {
                throw;
            }
            var result = new PagedResult<TimingViewModel>
            {
                Data = vmList,
                TotalItems = totalcount,
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            return result;
        }

        public IEnumerable<TimingViewModel> GetAll()
        {
            var timingList = _unitOfWork.Repository<Timing>().GetAll().ToList();
            var vmList = ConvertModelToViewModelList(timingList);//error here
            return vmList;
        }

        public TimingViewModel GetTimingById(int TimingId)
        {
            var model = _unitOfWork.Repository<Timing>().GetById(TimingId);
            var vm = new TimingViewModel(model);
            return vm;
        }

        public void UpdateTiming(TimingViewModel timing)
        {
            var model = new TimingViewModel().ConvertViewModel(timing);

            var ModelById = _unitOfWork.Repository<Timing>().GetById(model.Id);

            ModelById.Id = timing.Id;
            ModelById.DoctorId = timing.DoctorId;
            ModelById.Status = timing.Status;
            ModelById.Duration = timing.Duration;
            ModelById.MorningShiftStartTime = timing.MorningShiftStartTime;
            ModelById.MorningShiftEndTime = timing.MorningShiftEndTime;
            ModelById.AfternoonShiftStartTime = timing.AfternoonShiftStartTime;
            ModelById.AfternoonShiftEndTime = timing.AfternoonShiftEndTime;

            _unitOfWork.Repository<Timing>().Update(ModelById);
            _unitOfWork.Save();
        }

        private List<TimingViewModel> ConvertModelToViewModelList(List<Timing> modelList)
        {
            return modelList.Select(x => new TimingViewModel(x)).ToList();
        }


    }
}
