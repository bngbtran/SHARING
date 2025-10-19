using Business;

namespace Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User? GetById(int id);
        User? GetByGmail(string gmail);
        IEnumerable<User> Search(string keyword); 
        void Add(User user);
        void Update(User user);
        void Delete(int id);
        void Save();
    }
}
