using Microsoft.EntityFrameworkCore;
using TechMove1._3.Application.Services;
using TechMove1._3.Domain.Interfaces;
using TechMove1._3.Infrastructure.Data;
using TechMove1._3.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(); // <-- Use MVC, not just RazorPages
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddHttpClient<ICurrencyStrategy, ExchangeRateStrategy>();
builder.Services.AddScoped<ServiceRequestService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // <-- Serve wwwroot/css, js, files

app.UseRouting();
app.UseAuthorization();

// Set default route to ContractController / Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Contract}/{action=Index}/{id?}");

app.Run();