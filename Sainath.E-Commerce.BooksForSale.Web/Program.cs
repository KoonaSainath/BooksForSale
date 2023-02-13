using Microsoft.EntityFrameworkCore;
using Sainath.E_Commerce.BooksForSale.DataAccess.DbContextClasses;
using Sainath.E_Commerce.BooksForSale.DataAccess.IRepositories;
using Sainath.E_Commerce.BooksForSale.DataAccess.Repositories;
using Sainath.E_Commerce.BooksForSale.Web.Configurations;
using Sainath.E_Commerce.BooksForSale.Web.Configurations.IConfigurations;
using Microsoft.AspNetCore.Identity;

string connectionStringKey = "BooksForSaleConnectionString";
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<BooksForSaleDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString(connectionStringKey));
});

builder.Services.AddDefaultIdentity<IdentityUser>()
    .AddEntityFrameworkStores<BooksForSaleDbContext>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IBooksForSaleConfiguration, BooksForSaleConfiguration>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}");

app.Run();
