using Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;

namespace RazorWeb.Pages.Cars
{
    public class DetailsModel : PageModel
    {
        private readonly ICarRepository _carRepo;

        public DetailsModel(ICarRepository carRepo)
        {
            _carRepo = carRepo;
        }

        public Car Car { get; set; }

        public IActionResult OnGet(int id)
        {
            Car = _carRepo.GetById(id);
            if (Car == null) return RedirectToPage("Index");
            return Page();
        }
    }
}
