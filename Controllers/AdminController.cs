using GiftOfTheGivers.Data;
using GiftOfTheGivers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace GiftOfTheGivers.Controllers
{
    [Authorize(AuthenticationSchemes = "YourCookieAuth", Roles = "Admin")]
    public class AdminController : Controller
    {
      

            private readonly AppDbContext _context;

            public AdminController(AppDbContext context)
            {
                _context = context;
            }

        // GET: /Admin/Dashboard - Main admin dashboard with stats
        [HttpGet]
        public IActionResult Dashboard()
        {
            var adminName = User.FindFirst("FullName")?.Value ?? "Admin";
            var adminRole = User.FindFirst("AdminRole")?.Value ?? "Administrator";

            ViewBag.AdminName = adminName;
            ViewBag.AdminRole = adminRole;

            // Update stats to include donations
            var stats = new
            {
                TotalVolunteers = _context.Volunteers.Count(),
                ActiveTasks = _context.VolunteerTasks.Count(t => t.Status == "Open"),
                PendingApplications = _context.TaskApplications.Count(a => a.Status == "Applied"),
                TotalDisasterReports = _context.DisasterReports.Count(),
                NewDisasterReports = _context.DisasterReports.Count(d => d.Status == "Reported"),
                TotalDonations = _context.Donations.Count(),
                PendingDonations = _context.Donations.Count(d => d.Status == "Pending")
            };

            ViewBag.Stats = stats;
            return View();
        }

        // GET: /Admin/Volunteers - Manage all volunteers
        [HttpGet]
            public IActionResult Volunteers()
            {
                var volunteers = _context.Volunteers
                    .Include(v => v.User)
                    .OrderBy(v => v.User.FirstName)
                    .ToList();

                return View(volunteers);
            }

            // GET: /Admin/Tasks - View and manage volunteer tasks
            [HttpGet]
        [HttpGet]
        // GET: /Admin/Tasks - View and manage volunteer tasks
        [HttpGet]
        public IActionResult Tasks()
        {
            var tasks = _context.VolunteerTasks
                .Include(t => t.DisasterReport) // This will work now with ReportID
                .OrderByDescending(t => t.TaskDate)
                .ToList();

            return View(tasks);
        }
        [HttpGet]
        public IActionResult CreateTask(int? disasterReportId = null)
        {
            // Get available disasters for the dropdown
            var availableDisasters = _context.DisasterReports
                .Where(d => d.Status == "Reported")
                .ToList();

            ViewBag.AvailableDisasters = availableDisasters;

            // If creating from a specific disaster report, pre-fill the form
            if (disasterReportId.HasValue)
            {
                var disaster = _context.DisasterReports.Find(disasterReportId.Value);
                if (disaster != null)
                {
                    var task = new VolunteerTask
                    {
                        Title = $"Disaster Relief: {disaster.Location}",
                        Description = $"Emergency response needed for {disaster.DisasterType}. {disaster.Description}",
                        Location = disaster.Location,
                        Category = "Emergency Response",
                        TaskDate = DateTime.Now.AddDays(1),
                        DurationHours = 8,
                        VolunteersNeeded = 10,
                        ReportID = disasterReportId.Value
                    };
                    return View(task);
                }
            }

            // Otherwise, return empty form
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTask(VolunteerTask task)
        {
            // Remove all complex logic - just save the task
            task.CreatedDate = DateTime.Now;
            task.Status = "Open";
            task.VolunteersAssigned = 0;

            _context.VolunteerTasks.Add(task);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Task created successfully!";
            return RedirectToAction("Tasks");
        }
        // GET: /Admin/Applications - Manage volunteer applications
        [HttpGet]
            public IActionResult Applications()
            {
                var applications = _context.TaskApplications
                    .Include(a => a.Volunteer)
                    .ThenInclude(v => v.User)
                    .Include(a => a.VolunteerTask)
                    .Where(a => a.Status == "Applied")
                    .OrderByDescending(a => a.AppliedDate)
                    .ToList();

                return View(applications);
            }

            // POST: /Admin/ApproveApplication - Approve a volunteer application
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult ApproveApplication(int applicationId)
            {
                var application = _context.TaskApplications
                    .Include(a => a.VolunteerTask)
                    .Include(a => a.Volunteer)
                    .FirstOrDefault(a => a.ApplicationID == applicationId);

                if (application != null)
                {
                    application.Status = "Approved";
                    application.ApprovedDate = DateTime.Now;

                    // Update task volunteer count
                    application.VolunteerTask.VolunteersAssigned++;

                    // Check if task is now filled
                    if (application.VolunteerTask.VolunteersAssigned >= application.VolunteerTask.VolunteersNeeded)
                    {
                        application.VolunteerTask.Status = "Filled";
                    }

                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Application approved successfully!";
                }

                return RedirectToAction("Applications");
            }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateReportStatus(int reportId, string newStatus)
        {
            var report = _context.DisasterReports.Find(reportId);
            if (report != null)
            {
                report.Status = newStatus;
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Report status updated!";
            }
            return RedirectToAction("Reports");
        }
        public IActionResult Reports()
        {
            var disasterReports = _context.DisasterReports
                .Include(d => d.User)
                .OrderByDescending(d => d.ReportDate)
                .ToList();

            return View(disasterReports);
        }
        // GET: /Admin/Donations - View all donations
        [HttpGet]
        [HttpGet]
        public IActionResult Donations()
        {
            var donations = _context.Donations
                .Include(d => d.User)
                .OrderByDescending(d => d.DonationDate)
                .ToList();

            return View(donations);
        }

        // POST: /Admin/UpdateDonationStatus - Update donation status
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateDonationStatus(int donationId, string newStatus)
        {
            var donation = _context.Donations.Find(donationId);
            if (donation != null)
            {
                donation.Status = newStatus;
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Donation status updated!";
            }
            return RedirectToAction("Donations");
        }
    }
    }