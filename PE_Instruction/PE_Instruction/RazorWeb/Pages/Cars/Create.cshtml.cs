using Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository;
using System.Linq;

namespace RazorWeb.Pages.Cars
{
    public class CreateModel : PageModel
    {
        private readonly ICarRepository _carRepo;
        private readonly IBrandRepository _brandRepo;

        public CreateModel(ICarRepository carRepo, IBrandRepository brandRepo)
        {
            _carRepo = carRepo;
            _brandRepo = brandRepo;
        }

        [BindProperty]
        public Car Car { get; set; } = new Car();

        public SelectList BrandList { get; set; } = new SelectList(Enumerable.Empty<Brand>());

        public bool IsStaff { get; set; } = false;

        public void OnGet()
        {
            SetBrandList();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                SetBrandList();
                return Page();
            }

            // Nếu là Staff, đảm bảo BrandId là của họ
            if (IsStaff)
            {
                var brandIdClaim = User.FindFirst("BrandId")?.Value;
                if (int.TryParse(brandIdClaim, out int staffBrandId))
                {
                    Car.BrandId = staffBrandId;
                }
            }

            _carRepo.Add(Car);
            _carRepo.Save();

            return RedirectToPage("Index");
        }

        private void SetBrandList()
        {
            var roleClaim = User.FindFirst("Role")?.Value;
            var brandIdClaim = User.FindFirst("BrandId")?.Value;

            if (roleClaim == ((int)UserRole.Staff).ToString() && int.TryParse(brandIdClaim, out int staffBrandId))
            {
                // Staff chỉ được Brand của họ
                Car.BrandId = staffBrandId;
                BrandList = new SelectList(_brandRepo.GetAll().Where(b => b.Id == staffBrandId), "Id", "Name");
                IsStaff = true;
            }
            else
            {
                // Admin chọn tất cả Brand
                BrandList = new SelectList(_brandRepo.GetAll(), "Id", "Name");
                IsStaff = false;
            }
        }
    }
}
