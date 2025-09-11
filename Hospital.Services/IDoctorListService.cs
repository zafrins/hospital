using Hospital.ViewModels;
using Hospitals.Utilities;

public interface IDoctorListService
{
    PagedResult<ApplicationUserViewModel> GetAllDoctor(int pageNumber, int pageSize);
    ApplicationUserViewModel GetDoctorById(string id);
    void CreateDoctor(ApplicationUserViewModel model);
    void UpdateDoctor(ApplicationUserViewModel model);
    void DeleteDoctor(string id);
}
