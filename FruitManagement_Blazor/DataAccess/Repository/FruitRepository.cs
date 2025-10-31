using DataAccess.Context;
using DataAccess.IRepository;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repository
{
    public class FruitRepository : IFruitRepository
    {

        //---------------------- Khai báo, mặc định phải khai báo i chang vậy ----------------------
        private readonly FruitContext _context;

        public FruitRepository(FruitContext context)
        {
            _context = context;
        }

        //-------------------------------------------------------------------------------------------
        //---------------------- Hàm để Get Data. Get All để hiển thị list. GetById để phục vụ cho Edit, Delete ----------------------

        public List<Fruits> GetAll() =>
            _context.Fruits.Where(f => !f.IsDeleted).ToList();

        public Fruits? GetById(int id) =>
            _context.Fruits.FirstOrDefault(f => f.Id == id && !f.IsDeleted); // Do đề này có bool isDeleted. Nếu không có thì không cần đoạn "!f.IsDeleted"

        //-------------------------------------------------------------------------------------------
        //---------------------- Hàm Add, không đổi ----------------------

        public void Add(Fruits fruit)
        {
            try
            {
                _context.Fruits.Add(fruit);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error !!!");
            }
        }

        //-------------------------------------------------------------------------------------------
        //---------------------- Hàm Update, không đổi ----------------------

        public void Update(Fruits fruit)
        {
            try
            {
                var existing = _context.Fruits.AsNoTracking().FirstOrDefault(f => f.Id == fruit.Id);
                if (existing == null)
                    throw new Exception("Not Found !!!");

                _context.Fruits.Update(fruit);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error !!!");
            }
        }

        //-------------------------------------------------------------------------------------------
        //---------------------- Hàm xóa mềm ----------------------
        // Xóa mềm là không xóa hoàn toàn khỏi DB mà nó chỉ đánh isDeleted = 1 và biến mất khỏi giao diện. Nhưng trong DB vẫn còn.
        // Dùng hàm xóa mềm này khi Models có dòng bool isDeleted.

        public void Delete(int id)
        {
            try
            {
                var fruit = _context.Fruits.FirstOrDefault(f => f.Id == id);
                if (fruit == null)
                    throw new Exception("Not Found !!!");

                fruit.IsDeleted = true;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error !!!");
            }
        }

        //-------------------------------------------------------------------------------------------
        //---------------------- Hàm tìm kiếm theo tên, để đó nếu cần dùng ----------------------

        public List<Fruits> SearchByName(string keyword)
        {
            return _context.Fruits
                // Nếu đề không có xóa mềm thì không cần đoạn "f => !f.IsDeleted"
                // Nếu đề yêu cầu Search theo Title, search theo Age thì chỉ cần đổi đoạn: f.Name.Contains(keyword).
                // VD: Seach theo Title thì chuyển thành ==> f.Title.Contains(keyword)
                .Where(f => !f.IsDeleted && f.Name.Contains(keyword))
                .ToList();
        }

        //-------------------------------------------------------------------------------------------
        //---------------------- Hàm khôi phục những sản phẩm đã bị xóa mềm ----------------------

        public void Restore(int id)
        {
            var fruit = _context.Fruits.FirstOrDefault(f => f.Id == id && f.IsDeleted);
            if (fruit != null)
            {
                fruit.IsDeleted = false;
                _context.SaveChanges();
            }
        }

        public List<Fruits> GetDeleted() => _context.Fruits.Where(f => f.IsDeleted).ToList();

        //-------------------------------------------------------------------------------------------
        //---------------------- Hàm xóa cứng, ngược lại với xóa mềm ----------------------
        //---------------------- Khi xóa cứng thì data sẽ bị xóa hoàn toàn khỏi DB và không thể khôi phục ----------------------
        public void HardDelete(int id)
        {
            try
            {
                var fruit = _context.Fruits.FirstOrDefault(f => f.Id == id);
                if (fruit == null)
                    throw new Exception("Not Found !!!");

                _context.Fruits.Remove(fruit);
                _context.SaveChanges();        
            }
            catch (Exception ex)
            {
                throw new Exception($"Error !!!");
            }
        }
    }
}
