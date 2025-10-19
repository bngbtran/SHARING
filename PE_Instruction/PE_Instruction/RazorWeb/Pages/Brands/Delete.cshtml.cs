using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using Business;

namespace RazorWeb.Pages.Brands
{
    public class DeleteModel : PageModel
    {
        private readonly IBrandRepository _brandRepo;

        public DeleteModel(IBrandRepository brandRepo)
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
            _brandRepo.Delete(Brand.Id);
            _brandRepo.Save();
            return RedirectToPage("Index");
        }
    }
}
