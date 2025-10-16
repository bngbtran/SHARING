using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using VoNguyenHoangKim.Common.Enums;
using VoNguyenHoangKim.Service.Services;
using System.Security.Claims;
using VoNguyenHoangKim.RazorPages.Hubs;
using VoNguyenHoangKim.Service.ValidateSend;

namespace VoNguyenHoangKim.RazorPages.Pages.Staff
{
    [Authorize(Roles = "Staff")]
    public class EditNewsModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly INewsArticleService _newsArticleService;
        private readonly ITagService _tagService;
        private readonly IHubContext<NewsHub> _hubContext;

        public EditNewsModel(
            ICategoryService categoryService,
            INewsArticleService newsArticleService,
            ITagService tagService,
            IHubContext<NewsHub> hubContext)
        {
            _categoryService = categoryService;
            _newsArticleService = newsArticleService;
            _tagService = tagService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public NewsArticleRequest NewsArticleRequest { get; set; }
        public SelectList Categories { get; set; }
        public SelectList Tags { get; set; }
        public string AccountId { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var response = await _newsArticleService.GetByIdAsync(id);
            if (!response.Success)
            {
                return NotFound();
            }

            NewsArticleRequest = new NewsArticleRequest
            {
                Title = response.NewsArticle.Title,
                Headline = response.NewsArticle.Headline,
                Content = response.NewsArticle.Content,
                NewsSource = response.NewsArticle.NewsSource,
                CreatedDate = response.NewsArticle.CreatedDate,
                ModifiedDate = response.NewsArticle.ModifiedDate,
                Status = response.NewsArticle.Status,
                CategoryId = response.NewsArticle.CategoryId,
                AccountId = response.NewsArticle.AccountId,
                UpdatedById = response.NewsArticle.UpdatedById,
                TagIds = response.NewsArticle.TagIds
            };

            Categories = new SelectList((await _categoryService.GetAllAsync()).Where(c => c.Status == Status.Active), "Id", "Name", NewsArticleRequest.CategoryId);
            Tags = new SelectList(await _tagService.GetAllAsync(), "Id", "Name", NewsArticleRequest.TagIds);
            AccountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            var article = await _newsArticleService.GetByIdAsync(id);
            if (article == null)
            {
                TempData["Error"] = "Article not found.";
                return RedirectToPage();
            }

            AccountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(AccountId))
            {
                ModelState.AddModelError("NewsArticleRequest.AccountId", "User ID not found.");
                Categories = new SelectList((await _categoryService.GetAllAsync()).Where(c => c.Status == Status.Active), "Id", "Name");
                Tags = new SelectList(await _tagService.GetAllAsync(), "Id", "Name");
                return Page();
            }

            NewsArticleRequest.AccountId = AccountId;
            NewsArticleRequest.UpdatedById = AccountId;
            NewsArticleRequest.ModifiedDate = DateTime.UtcNow;

            if (!ModelState.IsValid)
            {
                Categories = new SelectList((await _categoryService.GetAllAsync()).Where(c => c.Status == Status.Active), "Id", "Name", NewsArticleRequest.CategoryId);
                Tags = new SelectList(await _tagService.GetAllAsync(), "Id", "Name", NewsArticleRequest.TagIds);
                return Page();
            }

            var articleResponse = await _newsArticleService.GetByIdAsync(id);
            if (!articleResponse.Success)
            {
                return NotFound();
            }

            var response = await _newsArticleService.UpdateAsync(id, NewsArticleRequest);
            if (!response.Success)
            {
                ModelState.AddModelError("", "Error !!!");
                Categories = new SelectList((await _categoryService.GetAllAsync()).Where(c => c.Status == Status.Active), "Id", "Name", NewsArticleRequest.CategoryId);
                Tags = new SelectList(await _tagService.GetAllAsync(), "Id", "Name", NewsArticleRequest.TagIds);
                return Page();
            }

            await _hubContext.Clients.All.SendAsync("ReceiveNewsUpdate", "updated", article.NewsArticle.Id, article.NewsArticle.Title, article.NewsArticle.Status.ToString());
            return RedirectToPage("/Staff/ManageNews");
        }
    }
}