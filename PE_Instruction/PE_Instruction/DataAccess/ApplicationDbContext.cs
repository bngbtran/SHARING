using Business;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        // Khai báo các Class. Chữ màu trắng sẽ là tên bảng trong Database.
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<User> Users { get; set; }

        //--------------------------------------------------

        // Đoạn này thể hiện mối quan hệ của các Object.
        // Đa số sẽ chỉ khai báo các mối quan hệ 1 - nhiều
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Quan hệ 1 - nhiều
            // 1 Brand có thể có nhiều Cars
            modelBuilder.Entity<Brand>()                         // Làm việc với entity "Brand"
                .HasMany(b => b.Cars)                            // 1 Brand có thể có nhiều Car
                .WithOne(c => c.Brand)                           // Mỗi Car chỉ thuộc 1 Brand duy nhất
                .HasForeignKey(c => c.BrandId)                   // Trong bảng Car, cột "BrandId" là khóa ngoại
                .OnDelete(DeleteBehavior.Cascade);               // Khi xóa Brand → xóa luôn toàn bộ Car thuộc Brand đó (Cascade Delete)

            // Quan hệ 1 - nhiều
            // 1 Brand có thể có nhiều Users
            modelBuilder.Entity<Brand>()                         // Làm việc với entity "Brand"
                .HasMany(b => b.Users)                           // 1 Brand có thể có nhiều User (Staff)
                .WithOne(u => u.Brand)                           // Mỗi User thuộc về đúng 1 Brand
                .HasForeignKey(u => u.BrandId)                   // Trong bảng User, cột "BrandId" là khóa ngoại
                .OnDelete(DeleteBehavior.Restrict);              // Khi xóa Brand → KHÔNG cho phép xóa nếu vẫn còn User thuộc Brand (Restrict Delete)
        }
    }
}
