using Hospital.Models;
using Hospital.Repositories.Interfaces;
using Hospital.Services;
using Hospital.ViewModels;
using Hospitals.Utilities;

public class DoctorListService : IDoctorListService
{
    private readonly IUnitOfWork _unitOfWork;

    public DoctorListService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public PagedResult<ApplicationUserViewModel> GetAllDoctor(int pageNumber, int pageSize)
    {
        int skip = (pageNumber - 1) * pageSize;
        var doctors = _unitOfWork.Repository<ApplicationUser>()
            .GetAll(u => u.IsDoctor)
            .Skip(skip)
            .Take(pageSize)
            .ToList();

        int totalCount = _unitOfWork.Repository<ApplicationUser>()
            .GetAll(u => u.IsDoctor)
            .Count();

        var doctorViewModels = doctors.Select(d => new ApplicationUserViewModel(d)).ToList();

        return new PagedResult<ApplicationUserViewModel>
        {
            Data = doctorViewModels,
            TotalItems = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    public ApplicationUserViewModel GetDoctorById(string id)
    {
        var user = _unitOfWork.Repository<ApplicationUser>().GetById(id);
        return user != null && user.IsDoctor ? new ApplicationUserViewModel(user) : null;
    }

    public void CreateDoctor(ApplicationUserViewModel model)
    {
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString(),
            Name = model.Name,
            Email = model.Email,
            UserName = model.UserName,
            Address = model.Address,
            Gender = model.Gender,
            IsDoctor = true,
            Specialist = model.Specialist,
            DOB = model.DOB
        };

        _unitOfWork.Repository<ApplicationUser>().Add(user);
        _unitOfWork.Save();
    }

    public void UpdateDoctor(ApplicationUserViewModel model)
    {
        var user = _unitOfWork.Repository<ApplicationUser>().GetById(model.Id);
        if (user == null || !user.IsDoctor) return;

        user.Name = model.Name;
        user.Email = model.Email;
        user.UserName = model.UserName;
        user.Address = model.Address;
        user.Gender = model.Gender;
        user.Specialist = model.Specialist;
        user.DOB = model.DOB;

        _unitOfWork.Repository<ApplicationUser>().Update(user);
        _unitOfWork.Save();
    }

    public void DeleteDoctor(string id)
    {
        var user = _unitOfWork.Repository<ApplicationUser>().GetById(id);
        if (user != null && user.IsDoctor)
        {
            _unitOfWork.Repository<ApplicationUser>().Delete(user);
            _unitOfWork.Save();
        }
    }
}
