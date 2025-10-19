using Business;

namespace Repository
{
    public interface ICarRepository
    {
        IEnumerable<Car> GetAll();
        IEnumerable<Car> GetCarsByBrand(int brandId);
        IEnumerable<Car> Search(string keyword);
        Car? GetById(int id);
        void Add(Car car);
        void Update(Car car);
        void Delete(int id);
        void Save();
    }
}
