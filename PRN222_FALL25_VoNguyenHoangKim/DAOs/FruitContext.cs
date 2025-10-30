using Microsoft.EntityFrameworkCore;
using BOs;

namespace DAOs
{
    public class FruitContext : DbContext
    {
        public FruitContext(DbContextOptions<FruitContext> options)
            : base(options)
        {
        }

        public DbSet<Fruits> Fruits { get; set; }

       
        public FruitContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Đổi tên server với tên DB thôi là oke. Còn lại giữ nguyên cấu trúc này
                optionsBuilder.UseSqlServer("Server=localhost;Database=FruitDB;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }
    }
}
