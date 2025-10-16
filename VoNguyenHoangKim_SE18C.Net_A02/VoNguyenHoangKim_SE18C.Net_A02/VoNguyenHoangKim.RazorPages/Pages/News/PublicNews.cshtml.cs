using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VoNguyenHoangKim.Service.Services;
using VoNguyenHoangKim.Service.ValidateReceive;

namespace VoNguyenHoangKim.RazorPages.Pages.News
{
    [AllowAnonymous]
    public class PublicNewsModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;

        public PublicNewsModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        public IEnumerable<NewsArticleResponse> NewsArticles { get; set; } = new List<NewsArticleResponse>();

        public async Task OnGetAsync(string search)
        {
            var articlesDto = await _newsArticleService.GetAllAsync();

            articlesDto = articlesDto.Where(a => a.Status == Common.Enums.Status.Active);

            var newsResponses = articlesDto.Select(a => new NewsArticleResponse
            {
                Success = true,
                NewsArticle = a,
                Error = null
            });

            if (!string.IsNullOrEmpty(search))
            {
                newsResponses = newsResponses.Where(a =>
                    a.NewsArticle.Title.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    a.NewsArticle.Headline?.Contains(search, StringComparison.OrdinalIgnoreCase) == true ||
                    a.NewsArticle.NewsSource?.Contains(search, StringComparison.OrdinalIgnoreCase) == true);
            }

            NewsArticles = newsResponses;

            ViewData["CurrentSearch"] = search;
        }

    }
}
