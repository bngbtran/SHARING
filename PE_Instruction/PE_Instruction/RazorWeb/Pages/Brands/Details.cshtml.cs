using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using Business;

namespace RazorWeb.Pages.Brands
{
    public class DetailsModel : PageModel
    {
        private readonly BrandRepository _brandRepo;

        public DetailsModel(BrandRepository brandRepo)
        {
            _brandRepo = brandRepo;
        }

        public Brand Brand { get; set; } = new Brand();

        public IActionResult OnGet(int id)
        {
            var brand = _brandRepo.GetById(id);
            if (brand == null)
                return RedirectToPage("Index");

            Brand = brand;
            return Page();
        }
    }
}
