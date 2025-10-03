using GiftOfTheGivers.Data;
using GiftOfTheGivers.Models;
using GiftOfTheGivers.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;


namespace GiftOfTheGivers.Controllers
{
    public class DisasterReportController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public DisasterReportController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: DisasterReport/Index - Main page with all reports
        [HttpGet]
        public IActionResult Index(string status = "", string disasterType = "", DateTime? startDate = null, DateTime? endDate = null)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var disasterReports = _context.DisasterReports
                .Include(d => d.User)
                .OrderByDescending(d => d.ReportDate)
                .ToList();

            // Apply filters
            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                disasterReports = disasterReports.Where(d => d.Status == status).ToList();
            }

            if (!string.IsNullOrEmpty(disasterType) && disasterType != "All")
            {
                disasterReports = disasterReports.Where(d => d.DisasterType == disasterType).ToList();
            }

            if (startDate.HasValue)
            {
                disasterReports = disasterReports.Where(d => d.ReportDate >= startDate.Value).ToList();
            }

            if (endDate.HasValue)
            {
                disasterReports = disasterReports.Where(d => d.ReportDate <= endDate.Value).ToList();
            }

            var viewModel = new DisasterReportHistoryViewModel
            {
                Reports = disasterReports.Select(d => new DisasterReportHistoryViewModel
                {
                    ReportID = d.ReportID,
                    ReportDate = d.ReportDate,
                    Location = d.Location,
                    DisasterType = d.DisasterType,
                    SeverityLevel = d.SeverityLevel,
                    Description = d.Description,
                    PeopleAffected = d.PeopleAffected,
                    IncidentDate = d.IncidentDate,
                    ImageUrl = d.ImageUrl,
                    Status = d.Status
                }).ToList(),

                SelectedStatus = status,
                SelectedDisasterType = disasterType,
                StartDate = startDate,
                EndDate = endDate,

                // Calculate summary stats
                TotalReports = disasterReports.Count,
                ReportedCount = disasterReports.Count(d => d.Status == "Reported"),
                UnderReviewCount = disasterReports.Count(d => d.Status == "Under Review"),
                AssistanceDeployedCount = disasterReports.Count(d => d.Status == "Assistance Deployed"),
                ResolvedCount = disasterReports.Count(d => d.Status == "Resolved")
            };

            return View(viewModel);
        }

        // GET: DisasterReport/Create - Form to report new disaster
        [HttpGet]
        public IActionResult Create()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var model = new DisasterReportViewModel();
            return View(model);
        }

        // POST: DisasterReport/Create - Submit new disaster report
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DisasterReportViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                string imageUrl = null;
                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    // Save image to wwwroot/uploads
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.ImageFile.CopyToAsync(fileStream);
                    }

                    imageUrl = "/uploads/" + uniqueFileName;
                }

                var disasterReport = new DisasterReport
                {
                    UserID = Convert.ToInt32(userId),
                    Location = model.Location,
                    DisasterType = model.DisasterType,
                    SeverityLevel = model.SeverityLevel,
                    Description = model.Description,
                    PeopleAffected = model.PeopleAffected,
                    IncidentDate = model.IncidentDate,
                    ImageUrl = imageUrl,
                    ReportDate = DateTime.Now,
                    Status = "Reported"
                };

                _context.DisasterReports.Add(disasterReport);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Disaster report submitted successfully! We will review it shortly.";
                return RedirectToAction("Index");
            }

            return View(model);
        }

        // GET: DisasterReport/Details/{id} - View single report details
        [HttpGet]
        public IActionResult Details(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var report = _context.DisasterReports
                .Include(d => d.User)
                .FirstOrDefault(d => d.ReportID == id);

            if (report == null)
            {
                return NotFound();
            }

            var viewModel = new DisasterReportHistoryViewModel
            {
                ReportID = report.ReportID,
                ReportDate = report.ReportDate,
                Location = report.Location,
                DisasterType = report.DisasterType,
                SeverityLevel = report.SeverityLevel,
                Description = report.Description,
                PeopleAffected = report.PeopleAffected,
                IncidentDate = report.IncidentDate,
                ImageUrl = report.ImageUrl,
                Status = report.Status
            };

            return View(viewModel);
        }
    }
}