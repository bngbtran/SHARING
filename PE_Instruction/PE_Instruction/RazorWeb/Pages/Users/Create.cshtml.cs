using Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;

namespace RazorWeb.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly IUserRepository _userRepo;
        private readonly IBrandRepository _brandRepo;

        public CreateModel(IUserRepository userRepo, IBrandRepository brandRepo)
        {
            _userRepo = userRepo;
            _brandRepo = brandRepo;
        }

        [BindProperty]
        public User User { get; set; } = new();

        public IEnumerable<Brand> Brands { get; set; } = new List<Brand>();

        public void OnGet()
        {
            Brands = _brandRepo.GetAll();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Brands = _brandRepo.GetAll();
                return Page();
            }

            _userRepo.Add(User);
            _userRepo.Save();
            return RedirectToPage("Index");
        }
    }
}
