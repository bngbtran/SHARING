using Business;
using DataAccess;

namespace Repository
{
    public class BrandRepository : IBrandRepository
    {
        private readonly ApplicationDbContext _context;

        public BrandRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Brand> GetAll() => _context.Brands.ToList(); // Xem danh sách

        public Brand? GetById(int id) => _context.Brands.Find(id); // Xem chi tiết

        public IEnumerable<Brand> Search(string keyword) // Search
        {
            if (string.IsNullOrEmpty(keyword))
                return GetAll();

            keyword = keyword.ToLower();
            return _context.Brands
                .Where(b => b.Name.ToLower().Contains(keyword))
                .ToList();
        }

        public void Add(Brand brand) => _context.Brands.Add(brand); // Create
        public void Update(Brand brand) => _context.Brands.Update(brand); // Update
        public void Delete(int id) // Delete
        {
            var brand = GetById(id);
            if (brand != null)
                _context.Brands.Remove(brand);
        }

        public void Save() => _context.SaveChanges(); // Lưu
    }
}
