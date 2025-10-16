using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using VoNguyenHoangKim.Service.Services;
using VoNguyenHoangKim.Service.ValidateSend;

namespace VoNguyenHoangKim.RazorPages.Pages.Admin
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly IAccountService _accountService;

        public EditModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [BindProperty]
        public AccountEditRequest Input { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            var response = await _accountService.GetByIdAsync(id);
            if (!response.Success)
            {
                return NotFound();
            }

            Input = new AccountEditRequest
            {
                Id = response.Account.Id,
                Email = response.Account.Email,
                FullName = response.Account.FullName,
                Role = response.Account.Role
            };
            ViewData["AccountId"] = id;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                ViewData["AccountId"] = id;
                return Page();
            }

            Input.Id = id;
            var response = await _accountService.UpdateAsync(id, Input);
            if (!response.Success)
            {
                ModelState.AddModelError("", "Error !!!");
                ViewData["AccountId"] = id;
                return Page();
            }

            return RedirectToPage("./Index");
        }

        public async Task<IActionResult> OnPostDeleteAsync(string id)
        {
            var response = await _accountService.DeleteAsync(id);
            if (!response.Success)
            {
                TempData["Error"] = "Error !!!";
            }

            return RedirectToPage("./Index");
        }
    }
}