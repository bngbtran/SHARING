using Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;

namespace RazorWeb.Pages.Cars
{
    public class DeleteModel : PageModel
    {
        private readonly ICarRepository _carRepo;

        public DeleteModel(ICarRepository carRepo)
        {
            _carRepo = carRepo;
        }

        [BindProperty]
        public Car Car { get; set; }

        public IActionResult OnGet(int id)
        {
            Car = _carRepo.GetById(id);
            if (Car == null) return RedirectToPage("Index");
            return Page();
        }

        public IActionResult OnPost(int id)
        {
            _carRepo.Delete(id);
            _carRepo.Save();
            return RedirectToPage("Index");
        }
    }
}
