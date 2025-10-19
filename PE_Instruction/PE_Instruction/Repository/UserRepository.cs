using Business;
using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAll() // Xem danh sách
        {
            return _context.Users
                .Include(u => u.Brand)
                .ToList();
        }

        public User? GetById(int id) // Chi tiết
        {
            return _context.Users
                .Include(u => u.Brand)
                .FirstOrDefault(u => u.Id == id);
        }

        public User? GetByGmail(string gmail) // Lọc ra theo Gmail
        {
            return _context.Users
                .Include(u => u.Brand)
                .FirstOrDefault(u => u.Email == gmail);
        }

        public IEnumerable<User> Search(string keyword) // Tìm kiếm
        {
            if (string.IsNullOrEmpty(keyword))
                return GetAll();

            keyword = keyword.ToLower();
            return _context.Users
                .Include(u => u.Brand)
                .Where(u =>
                    u.Email.ToLower().Contains(keyword) ||
                    (u.Brand != null && u.Brand.Name.ToLower().Contains(keyword)) ||
                    (u.Role.ToString().ToLower().Contains(keyword)))
                .ToList();
        }

        public void Add(User user) => _context.Users.Add(user); // Tạo
        public void Update(User user) => _context.Users.Update(user); // Sửa

        public void Delete(int id) // Xóa
        {
            var user = GetById(id);
            if (user != null)
                _context.Users.Remove(user);
        }

        public void Save() => _context.SaveChanges(); // Lưu
    }
}
