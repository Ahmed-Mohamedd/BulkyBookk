using BulkyBook.BLL.Interfaces;
using BulkyBook.BLL.Repositories;
using BulkyBook.DAL.Context;
using BulkyBook.DAL.Entities;
using BulkyBook.PL.Helpers;
using BulkyBook.PL.Mappers;
using BulkyBook.PL.Utilities;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Stripe;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Singltone for our BulkyDb

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.Configure<StripeSetting>(builder.Configuration.GetSection("Stripe"));

// Add A dependency injection for Categoryrepo used addScoped
//builder.Services.AddScoped(typeof(ICategoryRepository) , typeof(CategoryRepository));
//builder.Services.AddScoped(typeof(ICoverTypeRepository) , typeof(CoverTypeRepository));
builder.Services.AddScoped(typeof(IProductRepository) , typeof(ProductRepository));
//builder.Services.AddScoped(typeof(IConfiguration) , typeof(Configuration));
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();

builder.Services.AddTransient<ProductPictureUrlResolver>();
builder.Services.AddAutoMapper(m => m.AddProfile(new ProductProfile()));
builder.Services.AddIdentity<ApplicationUser, IdentityRole>( options =>
{
    //options.Password.RequireDigit = true;
    //options.Password.RequireLowercase = true;
    //options.Password.RequireNonAlphanumeric = true;
    //options.Password.RequiredLength = 20;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "Identity/Account/Login";
        options.AccessDeniedPath = "Customer/Home/Error";
    });
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

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseAuthentication();

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{area=Identity}/{controller=Account}/{action=Login}/{id?}");

app.Run();
