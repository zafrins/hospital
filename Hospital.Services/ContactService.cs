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
    public class ContactService : IContactService
    {
        private IUnitOfWork _unitOfWork;
        public ContactService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void DeleteContact(int id)
        {
            var model = _unitOfWork.Repository<Contact>().GetById(id);
            _unitOfWork.Repository<Contact>().Delete(model);
            _unitOfWork.Save();
        }

        public PagedResult<ContactViewModel> GetAll(int pageNumber, int pageSize)
        {
            int totalCount;
            List<ContactViewModel> vmList = new List<ContactViewModel>();
            try
            {
                int ExcludeRecords = (pageSize * pageNumber) - pageSize;

                var modelList = _unitOfWork.Repository<Contact>()
                    .GetAll(includeProperties:"Hospital")
                    .Skip(ExcludeRecords)
                    .Take(pageSize)
                    .ToList();

                totalCount = _unitOfWork.Repository<Contact>().GetAll().Count();

                vmList = ConvertToContactViewModel(modelList);
            }
            catch (Exception)
            {
                throw;
            }

            var result = new PagedResult<ContactViewModel>
            {
                Data = vmList,
                TotalItems = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return result;
        }


        public void UpdateContact(ContactViewModel Contact)
        {
            var model = _unitOfWork.Repository<Contact>().GetById(Contact.Id);
            if (model != null)
            {
                model.Email = Contact.Email;
                model.Phone = Contact.Phone;
                model.HospitalId = Contact.HospitalInfoId;
                _unitOfWork.Repository<Contact>().Update(model);
                _unitOfWork.Save();
            }
        }

        public void InsertContact(ContactViewModel Contact)
        {
            var model = new Contact
            {
                Email = Contact.Email,
                Phone = Contact.Phone,
                HospitalId = Contact.HospitalInfoId
            };
            _unitOfWork.Repository<Contact>().Add(model);
            _unitOfWork.Save();
        }
        public List<ContactViewModel> ConvertToContactViewModel(List<Contact> modelList)
        {
            return modelList.Select(x => new ContactViewModel
            {
                Id = x.Id,
                Email = x.Email,
                Phone = x.Phone,
                HospitalInfoId = x.HospitalId
            }).ToList();
        }

        public ContactViewModel GetContactById(int ContactId)
        {
            var model = _unitOfWork.Repository<Contact>().GetById(ContactId);
            if (model == null)
                return null;
            return new ContactViewModel
            {
                Id = model.Id,
                Email = model.Email,
                Phone = model.Phone,
                HospitalInfoId = model.HospitalId
            };
        }
    }
}
