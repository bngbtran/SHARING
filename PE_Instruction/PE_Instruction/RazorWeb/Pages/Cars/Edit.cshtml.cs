using Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Repository;
using System.Linq;

namespace RazorWeb.Pages.Cars
{
    public class EditModel : PageModel
    {
        private readonly ICarRepository _carRepo;
        private readonly IBrandRepository _brandRepo;

        public EditModel(ICarRepository carRepo, IBrandRepository brandRepo)
        {
            _carRepo = carRepo;
            _brandRepo = brandRepo;
        }

        [BindProperty]
        public Car Car { get; set; } = new Car();

        public SelectList BrandList { get; set; } = new SelectList(Enumerable.Empty<Brand>());

        public bool IsStaff { get; set; } = false;

        public IActionResult OnGet(int id)
        {
            Car = _carRepo.GetById(id);
            if (Car == null) return RedirectToPage("Index");

            SetBrandList();
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                SetBrandList();
                return Page();
            }

            // Nếu là Staff, ép BrandId về của họ
            if (IsStaff)
            {
                var brandIdClaim = User.FindFirst("BrandId")?.Value;
                if (!string.IsNullOrEmpty(brandIdClaim) && int.TryParse(brandIdClaim, out int staffBrandId))
                {
                    Car.BrandId = staffBrandId;
                }
            }

            _carRepo.Update(Car);
            _carRepo.Save();
            return RedirectToPage("Index");
        }

        private void SetBrandList()
        {
            var roleClaim = User.FindFirst("Role")?.Value;
            var brandIdClaim = User.FindFirst("BrandId")?.Value;

            if (!string.IsNullOrEmpty(roleClaim) && roleClaim == ((int)UserRole.Staff).ToString()
                && !string.IsNullOrEmpty(brandIdClaim) && int.TryParse(brandIdClaim, out int staffBrandId))
            {
                // Staff chỉ được brand của họ
                Car.BrandId = staffBrandId;
                BrandList = new SelectList(_brandRepo.GetAll().Where(b => b.Id == staffBrandId), "Id", "Name");
                IsStaff = true;
            }
            else
            {
                // Admin chọn tất cả brand
                BrandList = new SelectList(_brandRepo.GetAll(), "Id", "Name", Car.BrandId);
                IsStaff = false;
            }
        }
    }
}
