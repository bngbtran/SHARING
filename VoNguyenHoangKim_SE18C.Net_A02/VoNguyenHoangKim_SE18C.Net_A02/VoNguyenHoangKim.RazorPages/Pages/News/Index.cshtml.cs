using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VoNguyenHoangKim.Service.Services;
using System.Security.Claims;
using VoNguyenHoangKim.Service.ValidateReceive;

namespace VoNguyenHoangKim.RazorPages.Pages.News
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;
        public string CurrentUserRole => User.FindFirst(ClaimTypes.Role)?.Value;

        public IndexModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        public IEnumerable<NewsArticleResponse> NewsArticles { get; set; }

        public async Task OnGetAsync(string search)
        {
            var articlesDto = await _newsArticleService.GetAllAsync();
            NewsArticles = articlesDto.Select(a => new NewsArticleResponse
            {
                Success = true,
                NewsArticle = a,
                Error = null
            });

            if (!string.IsNullOrEmpty(search))
            {
                NewsArticles = NewsArticles.Where(a =>
                    a.NewsArticle.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    a.NewsArticle.Headline?.Contains(search, StringComparison.OrdinalIgnoreCase) == true ||
                    a.NewsArticle.NewsSource?.Contains(search, StringComparison.OrdinalIgnoreCase) == true);
            }

            ViewData["CurrentSearch"] = search;
        }
    }
}