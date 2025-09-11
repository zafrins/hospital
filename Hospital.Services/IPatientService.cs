using Hospital.ViewModels;
using Hospitals.Utilities;

public interface IPatientService
{
    PagedResult<ApplicationUserViewModel> GetAll(int pageNumber, int pageSize);
    void Create(ApplicationUserViewModel model);
    ApplicationUserViewModel GetById(string id);
    void Update(ApplicationUserViewModel model);
    void Delete(string id);
}
