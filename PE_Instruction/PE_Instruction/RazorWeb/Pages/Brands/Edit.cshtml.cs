using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using Business;

namespace RazorWeb.Pages.Brands
{
    public class EditModel : PageModel
    {
        private readonly IBrandRepository _brandRepo;

        public EditModel(IBrandRepository brandRepo)
        {
            _brandRepo = brandRepo;
        }

        [BindProperty]
        public Brand Brand { get; set; } = new Brand();

        public IActionResult OnGet(int id)
        {
            var brand = _brandRepo.GetById(id);
            if (brand == null)
                return RedirectToPage("Index");

            Brand = brand;
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            _brandRepo.Update(Brand);
            _brandRepo.Save();
            return RedirectToPage("Index");
        }
    }
}
