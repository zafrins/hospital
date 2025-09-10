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

            if (SecretCode == "admin1234")
            {
                return Redirect("/admin/Hospitals"); // Directly redirect to Hospitals dashboard
            }

            ModelState.AddModelError(string.Empty, "Incorrect secret code");
            return Page();
        }
    }
}
