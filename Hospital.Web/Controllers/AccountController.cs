using Microsoft.AspNetCore.Mvc;

namespace Hospital.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult AdminLogout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }

}
