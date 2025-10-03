using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GiftOfTheGivers.Data;
using GiftOfTheGivers.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace GiftOfTheGivers.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly AppDbContext _context;

        public MessagesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /Messages - Volunteers see their messages
        [HttpGet]
        public IActionResult Index()
        {
            var messages = _context.Messages
                .Include(m => m.FromUser)
                .Where(m => m.MessageType == "AdminBroadcast") // Volunteers only see broadcasts
                .OrderByDescending(m => m.SentDate)
                .ToList();

            return View(messages);
        }

        // POST: /Messages/SendToAdmin - Volunteer sends message to admin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendToAdmin(string subject, string content)
        {
            var userId = GetCurrentUserID();

            var message = new Message
            {
                FromUserID = userId,
                Subject = subject,
                Content = content,
                MessageType = "VolunteerToAdmin",
                SentDate = DateTime.Now
            };

            _context.Messages.Add(message);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Message sent to admin!";
            return RedirectToAction("Index");
        }

        // GET: /Messages/Admin - Admin sees all messages
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Admin()
        {
            var messages = _context.Messages
                .Include(m => m.FromUser)
                .OrderByDescending(m => m.SentDate)
                .ToList();

            return View(messages);
        }

        // POST: /Messages/Broadcast - Admin broadcasts to all volunteers
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult Broadcast(string subject, string content)
        {
            var message = new Message
            {
                Subject = subject,
                Content = content,
                MessageType = "AdminBroadcast",
                SentDate = DateTime.Now
            };

            _context.Messages.Add(message);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Message broadcasted to all volunteers!";
            return RedirectToAction("Admin");
        }

        private int? GetCurrentUserID()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId != null ? Convert.ToInt32(userId) : null;
        }
    }
}