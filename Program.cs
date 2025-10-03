using GiftOfTheGivers.Data;
using GiftOfTheGivers.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
});

// Add session services
// Add session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjust the timeout as needed
    options.Cookie.HttpOnly = true; // Ensure the cookie is not accessible via JavaScript
    options.Cookie.IsEssential = true; // Required for session to work in non-essential scenarios
});


// Add authentication services
builder.Services.AddAuthentication("YourCookieAuth")
    .AddCookie("YourCookieAuth", options =>
    {
        options.LoginPath = "/Account/Login"; // Redirect here for login
        options.LogoutPath = "/Account/Logout"; // Redirect here for logout
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

// Use session middleware before authentication and authorization
app.UseSession(); // Enable session support

app.UseAuthentication(); // Enable authentication middleware
app.UseAuthorization(); // Enable authorization middleware

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.MapControllerRoute(
    name: "messages",
    pattern: "Messages/{action=Index}/{id?}",
    defaults: new { controller = "Messages" });

app.Run();

// Add admin authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/AdminAuth/Login";
        options.AccessDeniedPath = "/AdminAuth/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(2);
    });

// Add authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

builder.Services.AddScoped<ReceiptPdfService>();
