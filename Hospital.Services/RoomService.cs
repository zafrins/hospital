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
    public class RoomService : IRoomService
    {
        private IUnitOfWork _unitOfWork;
        public RoomService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void DeleteRoom(int id)
        {
            var model = _unitOfWork.Repository<Room>().GetById(id);
            _unitOfWork.Repository<Room>().Delete(model);
            _unitOfWork.Save();
        }

        public PagedResult<RoomViewModel> GetAll(int pageNumber, int pageSize)
        {
            var vm = new RoomViewModel();
            int totalCount;
            List<RoomViewModel> vmList = new List<RoomViewModel>();
            try
            {
                int ExcludeRecords = (pageSize * pageNumber) - pageSize;

                var modelList = _unitOfWork.Repository<Room>()
                    .GetAll(includeProperties:"Hospital")
                    .Skip(ExcludeRecords)
                    .Take(pageSize)
                    .ToList();

                totalCount = _unitOfWork.Repository<Room>().GetAll().Count();

                vmList = ConvertToViewModel(modelList); // <-- Use existing method

            }
            catch (Exception)
            {
                throw;
            }

            var result = new PagedResult<RoomViewModel>
            {
                Data = vmList,
                TotalItems = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return result;
        }

        public RoomViewModel GetRoomById(int RoomId)
        {
            var model = _unitOfWork.Repository<Room>().GetById(RoomId);
            var vm = new RoomViewModel(model);
            return vm;

        }

        public void InsertRoom(RoomViewModel Room)
        {
            var model = new RoomViewModel().ConvertViewModel(Room);
            _unitOfWork.Repository<Room>().Add(model);
            _unitOfWork.Save();
        }

        public void UpdateRoom(RoomViewModel Room)
        {
            var model = new RoomViewModel().ConvertViewModel(Room);
            var ModelById = _unitOfWork.Repository<Room>().GetById(model.Id);
            ModelById.RoomNumber = Room.RoomNumber;
            ModelById.Type = Room.Type;
            ModelById.Status = Room.Status;
            ModelById.HospitalId = Room.HospitalInfoId;

            _unitOfWork.Repository<Room>().Update(ModelById);
            _unitOfWork.Save();
        }

        public List<RoomViewModel> ConvertToViewModel(List<Room> modelList)
        {
            return modelList.Select(x => new RoomViewModel(x)).ToList();
        }
    }
}
