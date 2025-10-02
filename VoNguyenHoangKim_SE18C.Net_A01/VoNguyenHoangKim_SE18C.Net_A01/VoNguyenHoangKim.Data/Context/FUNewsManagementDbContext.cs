using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using VoNguyenHoangKim.Common.Enums;
using VoNguyenHoangKim.Data.Models;

namespace VoNguyenHoangKim.Data.Context
{
    public class FUNewsManagementDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public FUNewsManagementDbContext(DbContextOptions<FUNewsManagementDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<NewsArticle> NewsArticles { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<NewsTag> NewsTags { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().HasKey(a => a.Id);
            modelBuilder.Entity<NewsArticle>().HasKey(n => n.Id);
            modelBuilder.Entity<Category>().HasKey(c => c.Id);
            modelBuilder.Entity<Tag>().HasKey(t => t.Id);
            modelBuilder.Entity<NewsTag>().HasKey(nt => nt.Id);

            modelBuilder.Entity<NewsArticle>()
                .HasOne(n => n.Category)
                .WithMany(c => c.NewsArticles)
                .HasForeignKey(n => n.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NewsArticle>()
                .HasOne(n => n.Account)
                .WithMany(a => a.NewsArticles)
                .HasForeignKey(n => n.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NewsArticle>()
                .HasOne(n => n.UpdatedBy)
                .WithMany()
                .HasForeignKey(n => n.UpdatedById)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<NewsTag>()
                .HasOne(nt => nt.NewsArticle)
                .WithMany(n => n.NewsTags)
                .HasForeignKey(nt => nt.NewsArticleId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NewsTag>()
                .HasOne(nt => nt.Tag)
                .WithMany(t => t.NewsTags)
                .HasForeignKey(nt => nt.TagId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Account>().HasData(
                new Account
                {
                    Id = "ACCOUNT_00000001",
                    Email = _configuration["DefaultAdmin:Email"],
                    Password = _configuration["DefaultAdmin:Password"],
                    Role = (int)Role.Admin,
                    FullName = "Administrator"
                });
        }
    }
}