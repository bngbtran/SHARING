using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VoNguyenHoangKim.Common.Enums;
using VoNguyenHoangKim.Service.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity.Data;

namespace VoNguyenHoangKim.RazorPages.Pages.Account
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        private readonly IAccountService _accountService;

        public LoginModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public LoginRequest Input { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var response = await _accountService.GetByEmailAsync(Input.Email);
            if (!response.Success || response.Account.Password != Input.Password)
            {
                ModelState.AddModelError("", "Invallid Email Or Password !!!");
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, response.Account.Id),
                new Claim(ClaimTypes.Email, response.Account.Email),
                new Claim(ClaimTypes.Role, ((Role)response.Account.Role).ToString()),
                new Claim(ClaimTypes.Name, response.Account.FullName)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToPage("/Home/Index");
        }
    }
}