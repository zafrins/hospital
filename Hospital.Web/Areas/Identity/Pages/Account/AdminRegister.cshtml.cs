using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Hospital.Web.Areas.Identity.Pages.Account
{
    public class AdminRegisterModel : PageModel
    {
        [BindProperty]
        [Required(ErrorMessage = "Please enter the secret code")]
        [Display(Name = "Secret Code")]
        public string SecretCode { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (SecretCode == "admin1234") // your secret code
            {
                // Save admin session
                HttpContext.Session.SetString("IsAdmin", "true");

                return Redirect("/Admin/Hospitals");
            }

            ModelState.AddModelError(string.Empty, "Incorrect secret code");
            return Page();
        }
    }
}
