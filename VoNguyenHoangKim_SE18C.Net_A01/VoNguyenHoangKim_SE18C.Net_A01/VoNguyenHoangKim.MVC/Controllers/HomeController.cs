using Microsoft.AspNetCore.Mvc;
using VoNguyenHoangKim.Common.Enums;
using VoNguyenHoangKim.Service.Services;

namespace VoNguyenHoangKim.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly INewsArticleService _newsArticleService;

        public HomeController(IAccountService accountService, INewsArticleService newsArticleService)
        {
            _accountService = accountService;
            _newsArticleService = newsArticleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var userResponse = await _accountService.GetByIdAsync(userId);
            if (!userResponse.Success)
            {
                return NotFound();
            }

            ViewBag.UserFullName = userResponse.Account.FullName;
            ViewBag.UserRole = userResponse.Account.Role.ToString();

            if (User.IsInRole("Admin"))
            {
                var totalAccounts = await _accountService.GetAllAsync();
                ViewBag.TotalAccounts = totalAccounts.Count();
                ViewBag.Message = "Welcome, Admin !!!";
            }
            else if (User.IsInRole("Staff"))
            {
                var newsCount = await _newsArticleService.GetAllAsync();
                ViewBag.TotalNews = newsCount.Count();
                ViewBag.Message = "Welcome, Staff !!!";
            }
            else if (User.IsInRole("Lecturer"))
            {
                var activeNews = await _newsArticleService.GetAllAsync();
                ViewBag.ActiveNewsCount = activeNews.Count(a => a.Status == Status.Active);
                ViewBag.Message = "Welcome, Lecturer !!!";
            }

            return View();
        }
    }
}