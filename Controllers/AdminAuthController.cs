using Microsoft.AspNetCore.Mvc;
using GiftOfTheGivers.Data;
using GiftOfTheGivers.ViewModels;
using GiftOfTheGivers.Models;
using BCrypt.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace GiftOfTheGivers.Controllers
{
    public class AdminAuthController : Controller
    {
        private readonly AppDbContext _context;

        public AdminAuthController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /AdminAuth/Login
        [HttpGet]
        public IActionResult Login()
        {
            // If already logged in as admin, redirect to dashboard
            if (User.Identity.IsAuthenticated && User.IsInRole("Admin"))
            {
                return RedirectToAction("Dashboard", "Admin");
            }

            return View();
        }

        // POST: /AdminAuth/Login
        // POST: /AdminAuth/Login
        // POST: /AdminAuth/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(AdminLoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Find admin by username or email
                var admin = _context.AdminUsers
                    .FirstOrDefault(a => a.Username == model.Username || a.Email == model.Username);

                // Check if admin exists and is active
                if (admin != null && admin.IsActive)
                {
                    // Update last login
                    admin.LastLogin = DateTime.Now;
                    _context.SaveChanges();

                    // Create claims for admin
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, admin.AdminID.ToString()),
                new Claim(ClaimTypes.Name, admin.Username),
                new Claim(ClaimTypes.Email, admin.Email),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim("AdminRole", admin.Role),
                new Claim("FullName", admin.FullName)
            };

                    // Use the same scheme name as your regular authentication
                    var claimsIdentity = new ClaimsIdentity(claims, "YourCookieAuth");
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    // Sign in admin using the same scheme
                    await HttpContext.SignInAsync("YourCookieAuth", claimsPrincipal);

                    // Redirect to admin dashboard
                    return RedirectToAction("Dashboard", "Admin");
                }

                ModelState.AddModelError(string.Empty, "Invalid admin credentials or account inactive.");
            }

            return View(model);
        }

        // POST: /AdminAuth/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("YourCookieAuth");
            return RedirectToAction("Login", "AdminAuth");
        }
    }
}