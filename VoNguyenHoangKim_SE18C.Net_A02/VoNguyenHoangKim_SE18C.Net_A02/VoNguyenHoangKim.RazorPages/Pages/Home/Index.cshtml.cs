using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VoNguyenHoangKim.Common.Enums;
using VoNguyenHoangKim.Service.Services;
using System.Security.Claims;

namespace VoNguyenHoangKim.RazorPages.Pages.Home
{
    public class IndexModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly INewsArticleService _newsArticleService;

        public IndexModel(IAccountService accountService, INewsArticleService newsArticleService)
        {
            _accountService = accountService;
            _newsArticleService = newsArticleService;
        }

        public string UserFullName { get; set; }
        public string UserRole { get; set; }
        public string Message { get; set; }
        public int TotalAccounts { get; set; }
        public int TotalNews { get; set; }
        public int ActiveNewsCount { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userResponse = await _accountService.GetByIdAsync(userId);
            if (!userResponse.Success)
            {
                return NotFound();
            }

            UserFullName = userResponse.Account.FullName;
            UserRole = userResponse.Account.Role.ToString();
            ViewData["UserFullName"] = UserFullName;
            ViewData["UserRole"] = UserRole;

            if (User.IsInRole("Admin"))
            {
                var totalAccounts = await _accountService.GetAllAsync();
                TotalAccounts = totalAccounts.Count();
                Message = "Welcome, Admin! Manage your system efficiently.";
                ViewData["TotalAccounts"] = TotalAccounts;
                ViewData["Message"] = Message;
            }
            else if (User.IsInRole("Staff"))
            {
                var newsCount = await _newsArticleService.GetAllAsync();
                TotalNews = newsCount.Count();
                Message = "Welcome, Staff! Manage categories, tags, and news.";
                ViewData["TotalNews"] = TotalNews;
                ViewData["Message"] = Message;
            }
            else if (User.IsInRole("Lecturer"))
            {
                var activeNews = await _newsArticleService.GetAllAsync();
                ActiveNewsCount = activeNews.Count(a => a.Status == Status.Active);
                Message = "Welcome, Lecturer! View and manage your news.";
                ViewData["ActiveNewsCount"] = ActiveNewsCount;
                ViewData["Message"] = Message;
            }

            return Page();
        }
    }
}