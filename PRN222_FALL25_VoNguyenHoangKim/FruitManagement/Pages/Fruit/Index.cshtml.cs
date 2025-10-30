using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using BOs;
using System.Collections.Generic;

namespace FruitManagement.Pages.CRUD
{
    public class IndexModel : PageModel
    {
        private readonly IFruitRepository _repo;
        public IndexModel(IFruitRepository repo)
        {
            _repo = repo;
        }

        public List<Fruits> Fruits { get; set; } = new();

        public void OnGet()
        {
            Fruits = _repo.GetAll();
        }
    }
}
