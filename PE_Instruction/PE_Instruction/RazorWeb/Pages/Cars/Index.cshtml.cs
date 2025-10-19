using Business;
using DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace RazorWeb.Pages.Cars
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        // Danh sách xe
        public List<Car> Cars { get; set; } = new();

        // Từ khóa tìm kiếm
        [BindProperty(SupportsGet = true)]
        public string? Keyword { get; set; }

        // Trang hiển thị
        public async Task OnGetAsync()
        {
            var query = _context.Cars.Include(c => c.Brand).AsQueryable();

            // Tìm kiếm theo từ khóa
            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                query = query.Where(c => c.Name.Contains(Keyword));
            }

            // Kiểm tra quyền
            if (User.Identity?.IsAuthenticated == true)
            {
                var brandIdClaim = User.FindFirst("BrandId")?.Value;
                if (int.TryParse(brandIdClaim, out int brandId))
                {
                    // Nếu có BrandId trong claim, filter theo BrandId
                    query = query.Where(c => c.BrandId == brandId);
                }
                // Nếu không có BrandId, là Admin toàn quyền -> xem tất cả
            }

            Cars = await query.ToListAsync();
        }
    }
}
