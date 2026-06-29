using Finn.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration
    .GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
        options.UseMySql(
            connectionString,
            ServerVersion.AutoDetect(connectionString)));

builder.Services
.AddDefaultIdentity<IdentityUser>(
options =>
{
    options.SignIn.RequireConfirmedAccount = false;

    options.Password.RequireDigit = false;

    options.Password.RequireLowercase = false;

    options.Password.RequireUppercase = false;

    options.Password.RequireNonAlphanumeric = false;

    options.Password.RequiredLength = 4;
})
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

builder.Services.AddServerSideBlazor();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern:
    "{controller=Auth}/{action=Login}/{id?}");

app.MapRazorPages();

var culture = new CultureInfo("hr-HR");



CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

1234.56m.ToString("N2");
app.Run();