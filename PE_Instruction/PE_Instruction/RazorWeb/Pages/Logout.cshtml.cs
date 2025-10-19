using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorWeb.Pages
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnPostAsync()
        {
            // Đăng xuất
            await HttpContext.SignOutAsync("MyCookieAuth");

            // Chuyển về trang Login
            return RedirectToPage("/Login");
        }
    }
}
