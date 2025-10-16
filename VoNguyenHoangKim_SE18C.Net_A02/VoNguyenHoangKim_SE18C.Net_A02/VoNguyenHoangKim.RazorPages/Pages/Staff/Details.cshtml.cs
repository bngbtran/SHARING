using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VoNguyenHoangKim.Service.Services;
using VoNguyenHoangKim.Service.ValidateReceive;

namespace VoNguyenHoangKim.RazorPages.Pages.Staff
{
    [Authorize(Roles = "Staff")]
    public class DetailsModel : PageModel
    {
        private readonly INewsArticleService _newsArticleService;

        public DetailsModel(INewsArticleService newsArticleService)
        {
            _newsArticleService = newsArticleService;
        }

        public NewsArticleResponse NewsArticle { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var response = await _newsArticleService.GetByIdAsync(id);
            if (!response.Success)
            {
                return NotFound();
            }

            NewsArticle = response;
            return Page();
        }
    }
}