using BOs;

namespace Repository
{
    // Hàm này để khai báo tất cả những action bên Repository. Bên Repository có bao nhiêu function thì bên này phải khai báo đủ .
    // Để ý nó là hàm void hay không, chứ khai báo sai/khác so với bên Repository thì nó cũng không chạy.
    public interface IFruitRepository
    {
        List<Fruits> GetAll();
        Fruits? GetById(int id);
        void Add(Fruits fruit);
        void Update(Fruits fruit);
        void Delete(int id);
        List<Fruits> SearchByName(string keyword);
        void Restore(int id);
        List<Fruits> GetDeleted();
    }
}
