using Microsoft.EntityFrameworkCore;
using TechMove1._3.Application.Services;
using TechMove1._3.Domain.Interfaces;
using TechMove1._3.Infrastructure.Data;
using TechMove1._3.Infrastructure.Services;
using TechMove1._3.Application.Observers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<IServiceRequestRepository, ServiceRequestRepository>();

builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddHttpClient<ICurrencyStrategy, ExchangeRateStrategy>(client =>
{
    client.BaseAddress = new Uri("https://api.exchangerate-api.com/");
    client.Timeout = TimeSpan.FromSeconds(10);
});
builder.Services.AddScoped<ServiceRequestService>();
builder.Services.AddScoped<ContractService>();

// Observer pattern registration
builder.Services.AddSingleton<ContractSubject>();
builder.Services.AddSingleton<NotificationObserver>();

// Memory cache for exchange rates
builder.Services.AddMemoryCache();

var app = builder.Build();

// Attach observer(s) to subject after DI is built
var subject = app.Services.GetRequiredService<ContractSubject>();
var notifier = app.Services.GetRequiredService<NotificationObserver>();
subject.Attach(notifier);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Contract}/{action=Index}/{id?}");

app.Run();