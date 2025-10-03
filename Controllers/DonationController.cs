using GiftOfTheGivers.Data;
using GiftOfTheGivers.Models;
using GiftOfTheGivers.Services;
using GiftOfTheGivers.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace GiftOfTheGivers.Controllers
{
    public class DonationsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<DonationsController> _logger;

        // ONLY ONE CONSTRUCTOR
        public DonationsController(AppDbContext context, ILogger<DonationsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Donations/Donate
        [HttpGet]
        public IActionResult Donate()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            // CREATE A NEW INSTANCE - THIS IS CRITICAL!
            var model = new DonationViewModel();
            return View(model);
        }

        [HttpGet]
        public IActionResult DonationHistory(string status = "", string resourceType = "", DateTime? startDate = null, DateTime? endDate = null)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var userDonations = _context.Donations
                .Where(d => d.UserID == Convert.ToInt32(userId))
                .OrderByDescending(d => d.DonationDate)
                .ToList();

            // Apply filters
            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                userDonations = userDonations.Where(d => d.Status == status).ToList();
            }

            if (!string.IsNullOrEmpty(resourceType) && resourceType != "All")
            {
                userDonations = userDonations.Where(d => d.ResourceType == resourceType).ToList();
            }

            if (startDate.HasValue)
            {
                userDonations = userDonations.Where(d => d.DonationDate >= startDate.Value).ToList();
            }

            if (endDate.HasValue)
            {
                userDonations = userDonations.Where(d => d.DonationDate <= endDate.Value).ToList();
            }

            var viewModel = new DonationHistoryViewModel
            {
                // Use DonationItemViewModel instead of self-reference
                Donations = userDonations.Select(d => new DonationItemViewModel
                {
                    DonationID = d.DonationID,
                    DonationDate = d.DonationDate,
                    ResourceType = d.ResourceType,
                    Quantity = d.Quantity,
                    Description = d.Description,
                    Condition = d.Condition,
                    DeliveryMethod = d.DeliveryMethod,
                    Status = d.Status,
                    
                }).ToList(),

                SelectedStatus = status,
                SelectedResourceType = resourceType,
                StartDate = startDate,
                EndDate = endDate,

                // Calculate summary stats
                TotalDonations = userDonations.Count,
                PendingCount = userDonations.Count(d => d.Status == "Pending"),
                ApprovedCount = userDonations.Count(d => d.Status == "Approved"),
                ReceivedCount = userDonations.Count(d => d.Status == "Received"),
                DistributedCount = userDonations.Count(d => d.Status == "Distributed")
            };

            return View(viewModel);
        }
        public IActionResult DownloadDonationReceipt(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var donation = _context.Donations
                .Include(d => d.User)
                .FirstOrDefault(d => d.DonationID == id && d.UserID == Convert.ToInt32(userId));

            if (donation == null)
            {
                return NotFound();
            }

            // Generate receipt using our service
            var pdfService = new ReceiptPdfService();
            var receiptBytes = pdfService.GenerateDonationReceipt(donation);

            // Return as downloadable text file
            return File(receiptBytes, "text/plain", $"DonationReceipt-{donation.DonationID}.txt");
        }

        [HttpPost]
        public IActionResult CancelDonation(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Login", "Account");
            }

            var donation = _context.Donations
                .FirstOrDefault(d => d.DonationID == id && d.UserID == Convert.ToInt32(userId));

            if (donation == null)
            {
                return NotFound();
            }

            if (donation.Status != "Pending")
            {
                TempData["ErrorMessage"] = "Only pending donations can be cancelled.";
                return RedirectToAction("DonationHistory");
            }

            donation.Status = "Cancelled";
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Donation cancelled successfully.";
            return RedirectToAction("DonationHistory");
        }
        [HttpPost]
        public IActionResult Donate(DonationViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            // NO MANUAL VALIDATION FOR DELIVERY FIELDS!
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var donation = new Donation
                {
                    UserID = Convert.ToInt32(userId),
                    ResourceType = model.ResourceType,
                    Quantity = model.Quantity,
                    Description = model.Description,
                    Condition = model.Condition,
                    DeliveryMethod = model.DeliveryMethod,
                    
                  
                    DonationDate = DateTime.Now,
                    Status = "Pending"
                };

                _context.Donations.Add(donation);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Thank you for your donation!";
                return RedirectToAction("Donate");
            }

            return View(model);
        }
    }
    }
