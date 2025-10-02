using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace VoNguyenHoangKim.Data.Context
{
    public class FUNewsManagementDbContextFactory : IDesignTimeDbContextFactory<FUNewsManagementDbContext>
    {
        public FUNewsManagementDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../VoNguyenHoangKim.MVC"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<FUNewsManagementDbContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

            return new FUNewsManagementDbContext(optionsBuilder.Options, configuration);
        }
    }
}
