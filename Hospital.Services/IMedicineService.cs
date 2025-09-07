using Hospital.ViewModels;
using Hospitals.Utilities;

namespace Hospital.Services
{
    public interface IMedicineService
    {
        PagedResult<MedicineViewModel> GetAll(int pageNumber, int pageSize);
        MedicineViewModel GetMedicineById(int id);
        void InsertMedicine(MedicineViewModel vm);
        void UpdateMedicine(MedicineViewModel vm);
        void DeleteMedicine(int id);
    }
}
