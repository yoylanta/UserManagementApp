using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UserManagementApp.Core.Models.Identity;
using UserManagementApp.Infrastructure;
using UserManagementApp.Presentation.Module;

var builder = WebApplication.CreateBuilder(args);
builder.Host.ConfigureSerilog(builder.Configuration);
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

builder.Services.AddAuthorization();
builder.Services.AddControllers();

builder.Services.AddApplicationServices();

builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
    {
        options.Password.RequireDigit = false; 
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false; 
        options.Password.RequireUppercase = false; 
        options.Password.RequiredLength = 1;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.ApplyMigrations();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapRazorPages();
app.MapControllers();

app.MapControllerRoute(
    name: "logout",
    pattern: "/Account/Logout",
    defaults: new { controller = "Account", action = "Logout" });

app.MapControllerRoute(
    name: "login",
    pattern: "/Account/Login",
    defaults: new { controller = "Account", action = "Login" });


app.Run();
