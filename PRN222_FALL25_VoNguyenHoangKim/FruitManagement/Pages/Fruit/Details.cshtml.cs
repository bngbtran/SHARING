using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using BOs;

namespace FruitManagement.Pages.CRUD
{
    public class DetailsModel : PageModel
    {
        private readonly IFruitRepository _repo;

        public DetailsModel(IFruitRepository repo)
        {
            _repo = repo;
        }

        public Fruits Fruit { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            var fruit = _repo.GetById(id);
            if (fruit == null) return RedirectToPage("Index");

            Fruit = fruit;
            return Page();
        }
    }
}
