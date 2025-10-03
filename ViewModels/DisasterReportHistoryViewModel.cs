using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.ViewModels
{
    public class DisasterReportHistoryViewModel
    {
        public int ReportID { get; set; }

        [Display(Name = "Report Date")]
        public DateTime ReportDate { get; set; }

        public string Location { get; set; }

        [Display(Name = "Disaster Type")]
        public string DisasterType { get; set; }

        [Display(Name = "Severity Level")]
        public string SeverityLevel { get; set; }

        public string Description { get; set; }

        [Display(Name = "People Affected")]
        public int PeopleAffected { get; set; }

        [Display(Name = "Incident Date")]
        public DateTime? IncidentDate { get; set; }

        public string ImageUrl { get; set; }
        public string Status { get; set; }

        // For filtering
        public string SelectedStatus { get; set; }
        public string SelectedDisasterType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public List<DisasterReportHistoryViewModel> Reports { get; set; } = new List<DisasterReportHistoryViewModel>();

        // Summary stats
        public int TotalReports { get; set; }
        public int ReportedCount { get; set; }
        public int UnderReviewCount { get; set; }
        public int AssistanceDeployedCount { get; set; }
        public int ResolvedCount { get; set; }

        public List<string> StatusOptions => new List<string>
        {
            "All", "Reported", "Under Review", "Assistance Deployed", "Resolved"
        };

        public List<string> DisasterTypeOptions => new List<string>
        {
            "All", "Flood", "Earthquake", "Fire", "Storm", "Drought",
            "Conflict", "Disease Outbreak", "Other"
        };
    }
}
