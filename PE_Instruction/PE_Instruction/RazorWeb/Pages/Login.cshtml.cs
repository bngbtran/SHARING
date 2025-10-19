using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;

namespace RazorWeb.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IUserRepository _userRepo;

        public LoginModel(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [BindProperty]
        public string Gmail { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public IActionResult OnGet()
        {
            if (User.Identity?.IsAuthenticated == true)
                return RedirectToPage("/Index");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = _userRepo.GetByGmail(Gmail);

            if (user == null || user.Password != Password)
            {
                ErrorMessage = "Invalid credentials!";
                return Page();
            }

            // Tạo Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim("Role", user.Role.ToString()),
                new Claim("BrandId", user.BrandId?.ToString() ?? "")
            };

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("MyCookieAuth", principal);

            return RedirectToPage("/Index");
        }
    }
}
