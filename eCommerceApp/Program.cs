using eCommerceApp.Data;
using eCommerceApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>(options =>

    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<ShoppingCartService>();
builder.Services.AddScoped<RegisterService>();
builder.Services.AddScoped<LoginService>();

var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata")
{
    SeedData.Initialize(app.Services).Wait();
}

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

app.UseAuthorization();
app.UseAuthentication();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
