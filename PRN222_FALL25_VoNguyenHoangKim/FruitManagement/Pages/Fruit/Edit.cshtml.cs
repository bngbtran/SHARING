using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using BOs;

namespace FruitManagement.Pages.CRUD
{
    public class EditModel : PageModel
    {
        private readonly IFruitRepository _repo;

        public EditModel(IFruitRepository repo)
        {
            _repo = repo;
        }

        [BindProperty]
        public Fruits Fruit { get; set; } = new();

        public IActionResult OnGet(int id)
        {
            var fruit = _repo.GetById(id);
            if (fruit == null) return RedirectToPage("Index");

            Fruit = fruit;
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            _repo.Update(Fruit);
            return RedirectToPage("Index");
        }
    }
}
