using DataAccess.IRepository;
using DataAccess.Models;

namespace BusinessObject.Service
{
    public class FruitsService : IFruitsService
    {
        private readonly IFruitRepository _repo;

        public FruitsService(IFruitRepository repo)
        {
            _repo = repo;
        }

        // Lấy tất cả fruits (không xóa mềm)
        public Task<List<Fruits>> GetAllFruitsAsync()
        {
            return Task.FromResult(_repo.GetAll());
        }

        // Lấy fruit theo Id
        public Task<Fruits?> GetFruitByIdAsync(int id)
        {
            return Task.FromResult(_repo.GetById(id));
        }

        // Thêm mới fruit
        public Task AddFruitAsync(Fruits fruit)
        {
            _repo.Add(fruit);
            return Task.CompletedTask;
        }

        // Cập nhật fruit
        public Task UpdateFruitAsync(Fruits fruit)
        {
            _repo.Update(fruit);
            return Task.CompletedTask;
        }

        // Xóa mềm fruit
        public Task DeleteFruitAsync(int id)
        {
            _repo.Delete(id);
            return Task.CompletedTask;
        }

        // Tìm kiếm fruit theo tên
        public Task<List<Fruits>> SearchFruitsAsync(string keyword)
        {
            return Task.FromResult(_repo.SearchByName(keyword));
        }

        // Khôi phục fruit đã xóa mềm
        public Task RestoreFruitAsync(int id)
        {
            _repo.Restore(id);
            return Task.CompletedTask;
        }

        // Lấy danh sách fruit đã xóa
        public Task<List<Fruits>> GetDeletedFruitsAsync()
        {
            return Task.FromResult(_repo.GetDeleted());
        }

        // Xóa cứng fruit
        public Task HardDeleteFruitAsync(int id)
        {
            _repo.HardDelete(id);
            return Task.CompletedTask;
        }
    }
}
