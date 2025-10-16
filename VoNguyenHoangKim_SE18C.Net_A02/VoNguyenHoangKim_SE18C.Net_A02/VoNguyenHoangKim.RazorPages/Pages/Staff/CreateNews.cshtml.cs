using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using VoNguyenHoangKim.Common.Enums;
using VoNguyenHoangKim.RazorPages.Hubs;
using VoNguyenHoangKim.Service.Services;
using VoNguyenHoangKim.Service.ValidateSend;

namespace VoNguyenHoangKim.RazorPages.Pages.Staff
{
    [Authorize(Roles = "Staff")]
    public class CreateNewsModel : PageModel
    {
        private readonly ICategoryService _categoryService;
        private readonly INewsArticleService _newsArticleService;
        private readonly ITagService _tagService;
        private readonly IHubContext<NewsHub> _hubContext;

        public CreateNewsModel(
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

        public async Task OnGetAsync()
        {
            Categories = new SelectList(
                (await _categoryService.GetAllAsync()).Where(c => c.Status == Status.Active),
                "Id", "Name"
            );
            Tags = new SelectList(await _tagService.GetAllAsync(), "Id", "Name");
            AccountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            AccountId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(AccountId))
            {
                ModelState.AddModelError("NewsArticleRequest.AccountId", "User ID not found.");
                await LoadSelectListsAsync();
                return Page();
            }

            NewsArticleRequest.AccountId = AccountId;
            NewsArticleRequest.UpdatedById = null;
            NewsArticleRequest.CreatedDate = DateTime.UtcNow;

            ModelState.Remove("NewsArticleRequest.AccountId");
            ModelState.Remove("NewsArticleRequest.UpdatedById");

            if (!ModelState.IsValid)
            {
                await LoadSelectListsAsync();
                return Page();
            }

            var response = await _newsArticleService.CreateAsync(NewsArticleRequest);
            if (!response.Success)
            {
                ModelState.AddModelError("", response.Error ?? "Error !!!");
                await LoadSelectListsAsync();
                return Page();
            }

            await _hubContext.Clients.All.SendAsync(
                "ReceiveNewsUpdate",
                "created",
                response.NewsArticle.Id,
                response.NewsArticle.Title,
                response.NewsArticle.Status.ToString()
            );

            return RedirectToPage("/Staff/ManageNews");
        }

        private async Task LoadSelectListsAsync()
        {
            Categories = new SelectList(
                (await _categoryService.GetAllAsync()).Where(c => c.Status == Status.Active),
                "Id", "Name"
            );
            Tags = new SelectList(await _tagService.GetAllAsync(), "Id", "Name");
        }
    }
}