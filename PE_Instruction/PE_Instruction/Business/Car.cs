namespace Business
{
    public class Car
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int BrandId { get; set; }

        // Ở đây không dùng ICollection vì nó là quan hệ 1 - 1.
        // 1 Car chỉ có thể thuộc 1 Brand duy nhất.
        public Brand? Brand { get; set; }
    }
}
