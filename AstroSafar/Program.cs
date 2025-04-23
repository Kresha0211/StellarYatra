

using AstroSafar.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Rotativa.AspNetCore;
using AstroSafar.Hubs;
using Microsoft.AspNetCore.SignalR;



var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Redirect to login page if not authenticated
        options.AccessDeniedPath = "/Account/AccessDenied"; // Redirect if access is denied
    });
builder.Services.AddSignalR(); // Add this
builder.Services.AddControllersWithViews();


builder.Services.AddHttpClient();

builder.Services.AddDbContext<SpaceLearningDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSession();


builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<SpaceLearningDBContext>()   
    .AddDefaultTokenProviders();

builder.Services.Configure<RazorpaySettings>(builder.Configuration.GetSection("Razorpay"));



builder.Services.AddAuthorization();

var app = builder.Build();
app.MapHub<ChatHub>("/chathub"); // ✅ This maps your SignalR Hub

var env = app.Environment;
RotativaConfiguration.Setup(env.WebRootPath, "Rotativa/wkhtmltopdf");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapControllerRoute(
    name: "admin_exams",
    pattern: "admin/exam-reports",
    defaults: new { controller = "Admin", action = "Progress" });

app.Run();





