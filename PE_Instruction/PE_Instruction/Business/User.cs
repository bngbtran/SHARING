namespace Business
{
    public enum UserRole
    {
        Admin = 1,
        Staff = 2
    }

    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // Enum role (sẽ lưu int trong DB)
        public UserRole Role { get; set; }  

        // Staff chỉ thuộc 1 brand
        public int? BrandId { get; set; }   
        public Brand? Brand { get; set; }
    }
}
