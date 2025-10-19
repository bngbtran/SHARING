using Business;

namespace Repository
{
    public interface IBrandRepository
    {
        IEnumerable<Brand> GetAll();
        Brand? GetById(int id);
        IEnumerable<Brand> Search(string keyword);
        void Add(Brand brand);
        void Update(Brand brand);
        void Delete(int id);
        void Save();
    }
}
