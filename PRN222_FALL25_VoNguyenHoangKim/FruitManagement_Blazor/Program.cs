using FruitManagement_Blazor.Components;
using DAOs;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace FruitManagement_Blazor
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            // 🔹 Đăng ký DbContext
            builder.Services.AddDbContext<FruitContext>(options =>
                options.UseSqlServer("Server=localhost;Database=FruitDB;Trusted_Connection=True;TrustServerCertificate=True;"));

            // 🔹 Đăng ký Repository
            builder.Services.AddScoped<IFruitRepository, FruitRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}
