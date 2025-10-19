using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using Business;

namespace RazorWeb.Pages.Brands
{
    public class CreateModel : PageModel
    {
        private readonly IBrandRepository _brandRepo;

        public CreateModel(IBrandRepository brandRepo)
        {
            _brandRepo = brandRepo;
        }

        [BindProperty]
        public Brand Brand { get; set; } = new Brand();

        public void OnGet() { }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            _brandRepo.Add(Brand);
            _brandRepo.Save();
            return RedirectToPage("Index");
        }
    }
}
