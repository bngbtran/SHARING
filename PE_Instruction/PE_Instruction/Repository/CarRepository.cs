using Business;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class CarRepository : ICarRepository
    {
        private readonly ApplicationDbContext _context;

        public CarRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Car> GetAll() // Xem danh sách
        {
            return _context.Cars
                .Include(c => c.Brand)
                .ToList();
        }

        public IEnumerable<Car> GetCarsByBrand(int brandId) // Lọc xe theo Brand
        {
            return _context.Cars
                .Where(c => c.BrandId == brandId)
                .Include(c => c.Brand)
                .ToList();
        }

        public IEnumerable<Car> Search(string keyword) // Search
        {
            if (string.IsNullOrEmpty(keyword))
                return GetAll();

            keyword = keyword.ToLower();
            return _context.Cars
                .Include(c => c.Brand)
                .Where(c => c.Name.ToLower().Contains(keyword)
                         || c.Brand.Name.ToLower().Contains(keyword))
                .ToList();
        }

        public Car? GetById(int id) // Xem chi tiết
        {
            return _context.Cars
                .Include(c => c.Brand)
                .FirstOrDefault(c => c.Id == id);
        }

        public void Add(Car car) => _context.Cars.Add(car); // Create
        public void Update(Car car) => _context.Cars.Update(car); // Update

        public void Delete(int id) // Xóa
        {
            var car = GetById(id);
            if (car != null)
                _context.Cars.Remove(car);
        }

        public void Save() => _context.SaveChanges(); // Lưu
    }
}
