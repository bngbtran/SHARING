using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VoNguyenHoangKim.Service.Services;
using VoNguyenHoangKim.Service.ValidateReceive;

namespace VoNguyenHoangKim.RazorPages.Pages.Staff
{
    [Authorize(Roles = "Staff")]
    public class ManageTagsModel : PageModel
    {
        private readonly ITagService _tagService;

        public ManageTagsModel(ITagService tagService)
        {
            _tagService = tagService;
        }

        public IEnumerable<TagResponse> Tags { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Search { get; set; } 

        public async Task OnGetAsync()
        {
            var tagsDto = await _tagService.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(Search))
            {
                tagsDto = tagsDto
                    .Where(t => t.Name.Contains(Search, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }

            Tags = tagsDto.Select(t => new TagResponse
            {
                Success = true,
                Tag = t,
                Error = null
            });

            ViewData["CurrentSearch"] = Search;
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            var response = await _tagService.DeleteAsync(id);
            if (!response.Success)
            {
                TempData["Error"] = "Error !!!";
            }

            return RedirectToPage();
        }
    }
}
