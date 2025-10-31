using DataAccess.Context;
using DataAccess.Repository;
using DataAccess.IRepository;
using BusinessObject.Service;
using Microsoft.EntityFrameworkCore;
using eStore.Components;

var builder = WebApplication.CreateBuilder(args);

// Import các Service cần thiết.
builder.Services.AddRazorComponents().AddInteractiveServerComponents();
builder.Services.AddScoped<FruitContext>();
builder.Services.AddScoped<IFruitRepository, FruitRepository>();
builder.Services.AddScoped<IFruitsService, FruitsService>();
builder.Services.AddHttpContextAccessor();

var conString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<FruitContext>(options =>
    options.UseSqlServer(conString));

builder.Services.AddQuickGridEntityFrameworkAdapter();

// File này cũng giữ nguyên luôn.
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var app = builder.Build();

// Đoạn này như cũ thôi, không đổi gì hết.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.Run();
