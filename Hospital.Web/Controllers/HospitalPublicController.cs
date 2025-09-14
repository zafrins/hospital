using Hospital.Services;
using Microsoft.AspNetCore.Mvc;

public class HospitalPublicController : Controller
{
    private readonly IHospitalInfo _hospitalService;

    public HospitalPublicController(IHospitalInfo hospitalService)
    {
        _hospitalService = hospitalService;
    }

    public IActionResult Index()
    {
        var result = _hospitalService.GetAll(1, 100); // Page 1, 100 items per page

        // Pass the list of hospitals, not the entire PagedResult
        return View(result.Data);
    }


    public IActionResult Details(int id)
    {
        var hospital = _hospitalService.GetHospitalById(id);
        if (hospital == null)
        {
            return NotFound();
        }
        return View(hospital);
    }
}
