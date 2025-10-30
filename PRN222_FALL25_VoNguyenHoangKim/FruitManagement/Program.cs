using DAOs;
using Microsoft.EntityFrameworkCore;
using Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// Chỗ này cũng giữ nguyên cấu trúc của String kết nối với SQL Server. Đổi tên DB với tên Server thôi.
builder.Services.AddDbContext<FruitContext>(options =>
    options.UseSqlServer("Server=localhost;Database=FruitDB;Trusted_Connection=True;TrustServerCertificate=True;"));

builder.Services.AddScoped<IFruitRepository, FruitRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.MapRazorPages();

app.Run();
