using Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;

namespace RazorWeb.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly IUserRepository _userRepo;

        public IndexModel(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        public IEnumerable<User> Users { get; set; } = new List<User>();

        [BindProperty(SupportsGet = true)]
        public string? Keyword { get; set; }

        public void OnGet()
        {
            Users = string.IsNullOrEmpty(Keyword)
                ? _userRepo.GetAll()
                : _userRepo.Search(Keyword);
        }
    }
}
