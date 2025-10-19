using Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;

namespace RazorWeb.Pages.Users
{
    public class DetailsModel : PageModel
    {
        private readonly IUserRepository _userRepo;

        public DetailsModel(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public User? User { get; set; }

        public IActionResult OnGet(int id)
        {
            User = _userRepo.GetById(id);
            if (User == null)
                return RedirectToPage("Index");

            return Page();
        }
    }
}
