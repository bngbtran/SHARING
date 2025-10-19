namespace Business
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }


        // Hai dòng dưới thể hiện mối quan hệ đê tạo DB.
        // 1 Brand có thể chứa nhiều Cars, 1 Brand có thể có nhiều Staffs quản lý.
        // ICollection thể hiện 1 tập hợp ==> Số nhiều
        public ICollection<Car> Cars { get; set; } = new List<Car>();
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
