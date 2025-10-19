using Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;

namespace RazorWeb.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly IUserRepository _userRepo;
        private readonly IBrandRepository _brandRepo;

        public EditModel(IUserRepository userRepo, IBrandRepository brandRepo)
        {
            _userRepo = userRepo;
            _brandRepo = brandRepo;
        }

        [BindProperty]
        public User User { get; set; } = new();

        public IEnumerable<Brand> Brands { get; set; } = new List<Brand>();

        public IActionResult OnGet(int id)
        {
            User = _userRepo.GetById(id);
            if (User == null) return RedirectToPage("Index");

            Brands = _brandRepo.GetAll();
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Brands = _brandRepo.GetAll();
                return Page();
            }

            // Lấy user từ DB
            var userInDb = _userRepo.GetById(User.Id);
            if (userInDb == null)
            {
                return RedirectToPage("Index");
            }

            // Nếu password không rỗng thì cập nhật, còn rỗng thì giữ nguyên
            if (!string.IsNullOrWhiteSpace(User.Password))
            {
                userInDb.Password = User.Password;
            }

            userInDb.Email = User.Email;
            userInDb.Role = User.Role;
            userInDb.BrandId = User.BrandId;

            _userRepo.Update(userInDb);
            _userRepo.Save();

            return RedirectToPage("Index");
        }

    }
}
