using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VoNguyenHoangKim.Common.Enums;
using VoNguyenHoangKim.Service.Services;
using VoNguyenHoangKim.Service.ValidateReceive;

namespace VoNguyenHoangKim.MVC.Controllers
{
    [Authorize(Roles = "Lecturer")]
    public class NewsController : Controller
    {
        private readonly INewsArticleService _newsArticleService;

        public NewsController(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search)
        {
            var articles = await _newsArticleService.GetAllAsync();
            var activeArticles = articles.Where(a => a.Status == Status.Active);
            if (!string.IsNullOrEmpty(search))
            {
                activeArticles = activeArticles.Where(a => a.Title.Contains(search, StringComparison.OrdinalIgnoreCase));
            }
            var responseList = activeArticles.Select(a => new NewsArticleResponse
            {
                Success = true,
                NewsArticle = a,
                Error = null
            }).ToList();

            return View(responseList);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            var response = await _newsArticleService.GetByIdAsync(id);
            if (!response.Success)
            {
                return NotFound();
            }
            return View(response);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> PublicNews(string search)
        {
            var articles = await _newsArticleService.GetAllAsync();
            var activeArticles = articles.Where(a => a.Status == Status.Active);

            if (!string.IsNullOrEmpty(search))
            {
                activeArticles = activeArticles.Where(a => a.Title.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            var responseList = activeArticles.Select(a => new NewsArticleResponse
            {
                Success = true,
                NewsArticle = a,
                Error = null
            }).ToList();

            return View(responseList);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> PublicDetails(string id)
        {
            var response = await _newsArticleService.GetByIdAsync(id);

            if (!response.Success || response.NewsArticle.Status != Status.Active)
            {
                return NotFound();
            }

            return View(response);
        }

    }
}