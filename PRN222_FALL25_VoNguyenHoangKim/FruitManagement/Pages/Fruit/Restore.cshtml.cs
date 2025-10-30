using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using BOs;
using System.Collections.Generic;

namespace FruitManagement.Pages.CRUD
{
    public class RestoreModel : PageModel
    {
        private readonly IFruitRepository _repo;
        public RestoreModel(IFruitRepository repo)
        {
            _repo = repo;
        }

        public List<Fruits> DeletedFruits { get; set; } = new();

        public void OnGet()
        {
            DeletedFruits = _repo.GetDeleted();
        }

        public IActionResult OnPost(int id)
        {
            _repo.Restore(id);
            return RedirectToPage();
        }
    }
}
