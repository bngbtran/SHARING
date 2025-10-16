using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VoNguyenHoangKim.Service.Services;
using System.Security.Claims;
using VoNguyenHoangKim.Service.ValidateReceive;

namespace VoNguyenHoangKim.RazorPages.Pages.Staff
{
    [Authorize(Roles = "Staff")]
    public class HistoryModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;

        public HistoryModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public IEnumerable<NewsArticleResponse> NewsArticles { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Account/Login");
            }

            var articlesDto = await _newsArticleService.GetAllAsync();

            var filtered = articlesDto
                .Where(a => a.AccountId == userId);

            if (!string.IsNullOrWhiteSpace(SearchTerm))
            {
                filtered = filtered
                    .Where(a => a.Title != null && a.Title.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            NewsArticles = filtered
                .Select(a => new NewsArticleResponse
                {
                    Success = true,
                    NewsArticle = a,
                    Error = null
                });

            return Page();
        }
    }
}
