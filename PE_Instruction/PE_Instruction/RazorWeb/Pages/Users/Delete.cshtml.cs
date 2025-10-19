using Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;

namespace RazorWeb.Pages.Users
{
    public class DeleteModel : PageModel
    {
        private readonly IUserRepository _userRepo;

        public DeleteModel(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [BindProperty]
        public User User { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            User = _userRepo.GetById(id);
            if (User == null) return RedirectToPage("Index");

            return Page();
        }

        public IActionResult OnPost()
        {
            _userRepo.Delete(User.Id);
            _userRepo.Save();
            return RedirectToPage("Index");
        }
    }
}
