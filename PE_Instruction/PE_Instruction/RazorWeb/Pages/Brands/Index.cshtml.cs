using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using Business;

namespace RazorWeb.Pages.Brands
{
    public class IndexModel : PageModel
    {
        private readonly IBrandRepository _brandRepo;

        public IndexModel(IBrandRepository brandRepo)
        {
            _brandRepo = brandRepo;
        }


        public IList<Brand> Brands { get; set; } = new List<Brand>();

        [BindProperty(SupportsGet = true)]
        public string? Keyword { get; set; }

        public void OnGet()
        {
            Brands = string.IsNullOrEmpty(Keyword)
                ? _brandRepo.GetAll().ToList()
                : _brandRepo.Search(Keyword).ToList();
        }
    }
}
