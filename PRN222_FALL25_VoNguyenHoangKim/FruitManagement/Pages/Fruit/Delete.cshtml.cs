using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using BOs;

namespace FruitManagement.Pages.CRUD
{
    public class DeleteModel : PageModel
    {
        private readonly IFruitRepository _repo;

        public DeleteModel(IFruitRepository repo)
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
            _repo.Delete(Fruit.Id);
            return RedirectToPage("Index");
        }
    }
}
