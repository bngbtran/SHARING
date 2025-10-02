using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VoNguyenHoangKim.Common.Enums;
using VoNguyenHoangKim.Service.Services;
using VoNguyenHoangKim.Service.ValidateReceive;
using VoNguyenHoangKim.Service.ValidateSend;

namespace VoNguyenHoangKim.MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IReportService _reportService;
        private readonly INewsArticleService _newsArticleService;

        public AdminController(IAccountService accountService, IReportService reportService, INewsArticleService newsArticleService)
        {
            _accountService = accountService;
            _reportService = reportService;
            _newsArticleService = newsArticleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search)
        {
            var accountsDto = await _accountService.GetAllAsync();

            if (!string.IsNullOrEmpty(search))
            {
                accountsDto = accountsDto.Where(a =>
                    a.Email.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    a.FullName.Contains(search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            var accounts = accountsDto.Select(a => new AccountResponse
            {
                Success = true,
                Account = a,
                Error = null
            });
            return View(accounts);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AccountCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var response = await _accountService.CreateAsync(request);
            if (!response.Success)
            {
                ModelState.AddModelError("", "Error !!!");
                return View(request);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var response = await _accountService.GetByIdAsync(id);
            if (!response.Success)
            {
                return NotFound();
            }

            var request = new AccountEditRequest
            {
                Id = response.Account.Id,
                Email = response.Account.Email,
                FullName = response.Account.FullName,
                Role = response.Account.Role
            };
            ViewBag.AccountId = id;
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, AccountEditRequest request)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.AccountId = id; 
                return View(request);
            }

            request.Id = id;

            var response = await _accountService.UpdateAsync(request.Id, request);
            if (!response.Success)
            {
                ModelState.AddModelError("", "Error !!!");
                ViewBag.AccountId = id;
                return View(request);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _accountService.DeleteAsync(id);
            if (!response.Success)
            {
                TempData["Error"] = "Error !!!";
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Report(DateTime? startDate, DateTime? endDate)
        {
            var articlesDto = await _newsArticleService.GetAllAsync();

            var filteredArticles = articlesDto.AsEnumerable();
            if (startDate.HasValue && endDate.HasValue)
            {
                var start = startDate.Value.Date;
                var end = endDate.Value.Date.AddDays(1).AddTicks(-1);
                filteredArticles = filteredArticles.Where(a => a.CreatedDate >= start && a.CreatedDate <= end);
            }
            else if (startDate.HasValue)
            {
                var start = startDate.Value.Date;
                filteredArticles = filteredArticles.Where(a => a.CreatedDate >= start);
            }
            else if (endDate.HasValue)
            {
                var end = endDate.Value.Date.AddDays(1).AddTicks(-1);
                filteredArticles = filteredArticles.Where(a => a.CreatedDate <= end);
            }

            var articles = filteredArticles.Select(a => new NewsArticleResponse
            {
                Success = true,
                NewsArticle = a,
                Error = null
            });

            var totalNews = articles.Count();
            var statsByCategory = articles.Select(a => a.NewsArticle)
                .GroupBy(a => a.CategoryName)
                .Select(g => new { Category = g.Key, Count = g.Count() })
                .ToList();

            var categoryLabels = statsByCategory.Select(s => s.Category).ToArray();
            var categoryData = statsByCategory.Select(s => s.Count).ToArray();
            var activeNewsCount = articles.Count(a => a.NewsArticle.Status == Status.Active);
            var inactiveNewsCount = articles.Count(a => a.NewsArticle.Status == Status.Inactive);

            var statsBySource = articles.Select(a => a.NewsArticle)
                .GroupBy(a => a.NewsSource ?? "Unknown")
                .Select(g => new { Source = g.Key, Count = g.Count() })
                .ToList();

            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            ViewBag.TotalNews = totalNews;
            ViewBag.StatsByCategory = statsByCategory;
            ViewBag.CategoryLabels = categoryLabels;
            ViewBag.CategoryData = categoryData;
            ViewBag.ActiveNewsCount = activeNewsCount;
            ViewBag.InactiveNewsCount = inactiveNewsCount;
            ViewBag.StatsBySource = statsBySource;

            var sortedArticles = articles.OrderByDescending(a => a.NewsArticle.CreatedDate).ToList();

            return View(articles);
        }
    }
}