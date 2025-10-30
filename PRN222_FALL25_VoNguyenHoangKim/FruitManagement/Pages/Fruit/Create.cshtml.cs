using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using BOs;

namespace FruitManagement.Pages.CRUD
{
    public class CreateModel : PageModel
    {
        private readonly IFruitRepository _repo;

        public CreateModel(IFruitRepository repo)
        {
            _repo = repo;
        }

        [BindProperty]
        public Fruits Fruit { get; set; } = new();

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();

            _repo.Add(Fruit);
            return RedirectToPage("Index");
        }
    }
}
