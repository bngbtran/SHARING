using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VoNguyenHoangKim.Service.Services;
using VoNguyenHoangKim.Service.ValidateReceive;

namespace VoNguyenHoangKim.RazorPages.Pages.Staff
{
    [Authorize(Roles = "Staff")]
    public class IndexModel : PageModel
    {
        private readonly ICategoryService _categoryService;

        public IndexModel(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IEnumerable<CategoryResponse> Categories { get; set; }

        public async Task OnGetAsync(string search)
        {
            var categoriesDto = await _categoryService.GetAllAsync();
            Categories = categoriesDto.Select(c => new CategoryResponse
            {
                Success = true,
                Category = c,
                Error = null
            });

            if (!string.IsNullOrEmpty(search))
            {
                Categories = Categories.Where(c => c.Category.Name.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            ViewData["CurrentSearch"] = search;
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            var response = await _categoryService.DeleteAsync(id);
            if (!response.Success)
            {
                TempData["Error"] = "Error !!!";
            }

            return RedirectToPage();
        }
    }
}