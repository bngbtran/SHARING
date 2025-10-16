using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VoNguyenHoangKim.Common.Enums;
using VoNguyenHoangKim.Service.Services;
using VoNguyenHoangKim.Service.ValidateReceive;

namespace VoNguyenHoangKim.RazorPages.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class ReportModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;

        public ReportModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        [BindProperty]
        public DateTime? StartDate { get; set; }
        [BindProperty]
        public DateTime? EndDate { get; set; }
        public IList<NewsArticleResponse> Articles { get; set; }
        public int TotalNews { get; set; }
        public int ActiveNewsCount { get; set; }
        public int InactiveNewsCount { get; set; }
        public CategoryStat[] StatsByCategory { get; set; } 
        public string[] CategoryLabels { get; set; }
        public int[] CategoryData { get; set; }
        public object[] StatsBySource { get; set; }

        public async Task OnGetAsync(DateTime? startDate, DateTime? endDate)
        {
            StartDate = startDate;
            EndDate = endDate;
            await LoadReportData();
        }

        public async Task OnPostAsync()
        {
            await LoadReportData();
        }

        private async Task LoadReportData()
        {
            var articlesDto = await _newsArticleService.GetAllAsync();
            var filteredArticles = articlesDto.AsEnumerable();

            if (StartDate.HasValue && EndDate.HasValue)
            {
                var start = StartDate.Value.Date;
                var end = EndDate.Value.Date.AddDays(1).AddTicks(-1);
                filteredArticles = filteredArticles.Where(a => a.CreatedDate >= start && a.CreatedDate <= end);
            }
            else if (StartDate.HasValue)
            {
                var start = StartDate.Value.Date;
                filteredArticles = filteredArticles.Where(a => a.CreatedDate >= start);
            }
            else if (EndDate.HasValue)
            {
                var end = EndDate.Value.Date.AddDays(1).AddTicks(-1);
                filteredArticles = filteredArticles.Where(a => a.CreatedDate <= end);
            }

            Articles = filteredArticles.Select(a => new NewsArticleResponse
            {
                Success = true,
                NewsArticle = a,
                Error = null
            }).ToList();

            TotalNews = Articles.Count;
            StatsByCategory = Articles.Select(a => a.NewsArticle)
                .GroupBy(a => a.CategoryName)
                .Select(g => new CategoryStat { Category = g.Key, Count = g.Count() }) 
                .ToArray();
            CategoryLabels = StatsByCategory.Select(s => s.Category).ToArray();
            CategoryData = StatsByCategory.Select(s => s.Count).ToArray();
            ActiveNewsCount = Articles.Count(a => a.NewsArticle.Status == Status.Active);
            InactiveNewsCount = Articles.Count(a => a.NewsArticle.Status == Status.Inactive);
            StatsBySource = Articles.Select(a => a.NewsArticle)
                .GroupBy(a => a.NewsSource ?? "Unknown")
                .Select(g => new { Source = g.Key, Count = g.Count() })
                .ToArray();

            ViewData["StartDate"] = StartDate;
            ViewData["EndDate"] = EndDate;
            ViewData["TotalNews"] = TotalNews;
            ViewData["StatsByCategory"] = StatsByCategory;
            ViewData["CategoryLabels"] = CategoryLabels;
            ViewData["CategoryData"] = CategoryData;
            ViewData["ActiveNewsCount"] = ActiveNewsCount;
            ViewData["InactiveNewsCount"] = InactiveNewsCount;
            ViewData["StatsBySource"] = StatsBySource;
        }
    }

    public class CategoryStat
    {
        public string Category { get; set; }
        public int Count { get; set; }
    }
}