using Microsoft.AspNetCore.Mvc;
using GiftOfTheGivers.Data;
using GiftOfTheGivers.Models;
using GiftOfTheGivers.ViewModels;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace GiftOfTheGivers.Controllers
{
    public class VolunteerController : Controller
    {
        private readonly AppDbContext _context;

        public VolunteerController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Volunteer/Register - First time registration
        [HttpGet]
        public IActionResult Register()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _context.Users.FirstOrDefault(u => u.UserID == Convert.ToInt32(userId));

            // Check if already registered as volunteer
            var existingVolunteer = _context.Volunteers.FirstOrDefault(v => v.UserID == Convert.ToInt32(userId));
            if (existingVolunteer != null)
            {
                return RedirectToAction("Hub");
            }

            var model = new VolunteerRegistrationViewModel();

            return View(model);
        }

        // POST: Volunteer/Register - Submit registration
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(VolunteerRegistrationViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var volunteer = new Volunteer
                {
                    UserID = Convert.ToInt32(userId), // Still link to logged-in user
                   
                    PhoneNumber = model.PhoneNumber,
                    EmergencyContact = model.EmergencyContact,
                    EmergencyContactPhone = model.EmergencyContactPhone,
                    Skills = string.Join(",", model.SelectedSkills),
                    PreviousExperience = model.PreviousExperience,
                    Availability = model.Availability,
                    PreferredTasks = string.Join(",", model.SelectedTaskTypes),
                    Status = "Active",
                    RegistrationDate = DateTime.Now,
                    LastUpdated = DateTime.Now
                };

                _context.Volunteers.Add(volunteer);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Welcome to the Gift of the Givers volunteer team!";
                return RedirectToAction("Hub");
            }

            return View(model);
        }

        // GET: Volunteer/Hub - Main dashboard
        [HttpGet]
        public IActionResult Hub()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var volunteer = _context.Volunteers.FirstOrDefault(v => v.UserID == Convert.ToInt32(userId));

            // Redirect to registration if not a volunteer yet
            if (volunteer == null)
            {
                return RedirectToAction("Register");
            }

            var viewModel = new VolunteerHubViewModel();

            // Get stats
            var completedTasks = _context.TaskApplications
                .Where(ta => ta.VolunteerID == volunteer.VolunteerID && ta.Status == "Completed")
                .ToList();

            viewModel.TotalHours = completedTasks.Sum(ta =>
                _context.VolunteerTasks.First(t => t.TaskID == ta.TaskID).DurationHours);
            viewModel.TasksCompleted = completedTasks.Count;

            viewModel.UpcomingShifts = _context.TaskApplications
                .Count(ta => ta.VolunteerID == volunteer.VolunteerID &&
                           ta.Status == "Approved" &&
                           _context.VolunteerTasks.First(t => t.TaskID == ta.TaskID).TaskDate >= DateTime.Now);

            viewModel.PendingApplications = _context.TaskApplications
                .Count(ta => ta.VolunteerID == volunteer.VolunteerID && ta.Status == "Applied");

            // Get available tasks (not applied to, not full, future dates)
            var appliedTaskIds = _context.TaskApplications
                .Where(ta => ta.VolunteerID == volunteer.VolunteerID)
                .Select(ta => ta.TaskID)
                .ToList();

            var availableTasks = _context.VolunteerTasks
                .Where(t => t.TaskDate >= DateTime.Now &&
                          t.Status == "Open" &&
                          t.VolunteersAssigned < t.VolunteersNeeded &&
                          !appliedTaskIds.Contains(t.TaskID))
                .OrderBy(t => t.TaskDate)
                .ToList();

            viewModel.AvailableTasks = availableTasks.Select(t => new VolunteerTaskViewModel
            {
                TaskID = t.TaskID,
                Title = t.Title,
                Description = t.Description,
                Category = t.Category,
                Location = t.Location,
                RequiredSkills = t.RequiredSkills,
                TaskDate = t.TaskDate,
                DurationHours = t.DurationHours,
                VolunteersNeeded = t.VolunteersNeeded,
                VolunteersAssigned = t.VolunteersAssigned,
                Status = t.Status
            }).ToList();

            // Get my schedule (approved tasks)
            var myApplications = _context.TaskApplications
                .Where(ta => ta.VolunteerID == volunteer.VolunteerID && ta.Status == "Approved")
                .Include(ta => ta.VolunteerTask)
                .Where(ta => ta.VolunteerTask.TaskDate >= DateTime.Now)
                .OrderBy(ta => ta.VolunteerTask.TaskDate)
                .ToList();

            viewModel.MySchedule = myApplications.Select(ta => new VolunteerTaskViewModel
            {
                TaskID = ta.TaskID,
                Title = ta.VolunteerTask.Title,
                Description = ta.VolunteerTask.Description,
                Category = ta.VolunteerTask.Category,
                Location = ta.VolunteerTask.Location,
                TaskDate = ta.VolunteerTask.TaskDate,
                DurationHours = ta.VolunteerTask.DurationHours,
                ApplicationStatus = ta.Status,
                ApplicationID = ta.ApplicationID
            }).ToList();

            return View(viewModel);
        }

        // POST: Volunteer/Apply - Apply for a task
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Apply(int taskId, string applicationNotes)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var volunteer = _context.Volunteers.FirstOrDefault(v => v.UserID == Convert.ToInt32(userId));

            if (volunteer == null)
            {
                return RedirectToAction("Register");
            }

            var existingApplication = _context.TaskApplications
                .FirstOrDefault(ta => ta.VolunteerID == volunteer.VolunteerID && ta.TaskID == taskId);

            if (existingApplication != null)
            {
                TempData["ErrorMessage"] = "You have already applied for this task.";
                return RedirectToAction("Hub");
            }

            var application = new TaskApplication
            {
                VolunteerID = volunteer.VolunteerID,
                TaskID = taskId,
                Status = "Applied",
                ApplicationNotes = applicationNotes,
                AppliedDate = DateTime.Now
            };

            _context.TaskApplications.Add(application);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Application submitted successfully!";
            return RedirectToAction("Hub");
        }

        // GET: Volunteer/Contributions - View personal contributions
        [HttpGet]
        public IActionResult Contributions()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var volunteer = _context.Volunteers.FirstOrDefault(v => v.UserID == Convert.ToInt32(userId));

            if (volunteer == null)
            {
                return RedirectToAction("Register");
            }

            var completedApplications = _context.TaskApplications
                .Where(ta => ta.VolunteerID == volunteer.VolunteerID && ta.Status == "Completed")
                .Include(ta => ta.VolunteerTask)
                .OrderByDescending(ta => ta.VolunteerTask.TaskDate)
                .ToList();

            var viewModel = new VolunteerContributionsViewModel
            {
                TotalHours = completedApplications.Sum(ta => ta.VolunteerTask.DurationHours),
                TotalTasks = completedApplications.Count,
                MemberSince = volunteer.RegistrationDate,
                HoursThisMonth = completedApplications
                    .Where(ta => ta.VolunteerTask.TaskDate.Month == DateTime.Now.Month)
                    .Sum(ta => ta.VolunteerTask.DurationHours),
                HoursThisYear = completedApplications
                    .Where(ta => ta.VolunteerTask.TaskDate.Year == DateTime.Now.Year)
                    .Sum(ta => ta.VolunteerTask.DurationHours),
                Contributions = completedApplications.Select(ta => new ContributionItem
                {
                    TaskTitle = ta.VolunteerTask.Title,
                    Category = ta.VolunteerTask.Category,
                    TaskDate = ta.VolunteerTask.TaskDate,
                    Hours = ta.VolunteerTask.DurationHours,
                    Status = ta.Status
                }).ToList()
            };

            // Find most active category
            if (completedApplications.Any())
            {
                viewModel.MostActiveCategory = completedApplications
                    .GroupBy(ta => ta.VolunteerTask.Category)
                    .OrderByDescending(g => g.Count())
                    .First().Key;
            }

            return View(viewModel);
        }
    }
}
