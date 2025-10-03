using GiftOfTheGivers.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;


namespace GiftOfTheGivers.ViewModels
{
    public class DonationHistoryViewModel
    {
        // For filtering
        public string SelectedStatus { get; set; }
        public string SelectedResourceType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        // Use DonationItemViewModel instead of self-reference
        public List<DonationItemViewModel> Donations { get; set; } = new List<DonationItemViewModel>();

        // Summary stats
        public int TotalDonations { get; set; }
        public int PendingCount { get; set; }
        public int ApprovedCount { get; set; }
        public int ReceivedCount { get; set; }
        public int DistributedCount { get; set; }

        public List<string> StatusOptions => new List<string>
        {
            "All", "Pending", "Approved", "Scheduled", "Received", "Distributed", "Cancelled"
        };

        public List<string> ResourceTypeOptions => new List<string>
        {
            "All", "Food & Water", "Clothing & Blankets", "Medical Supplies",
            "Hygiene Products", "Baby Items", "School Supplies", "Building Materials", "Other Essentials"
        };
    }
}