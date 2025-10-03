using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.ViewModels
{
    public class DisasterReportViewModel
    {
        public int ReportID { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [Display(Name = "Location")]
        public string Location { get; set; }

        [Required(ErrorMessage = "Please select disaster type")]
        [Display(Name = "Disaster Type")]
        public string DisasterType { get; set; }

        [Required(ErrorMessage = "Please select severity level")]
        [Display(Name = "Severity Level")]
        public string SeverityLevel { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please estimate number of people affected")]
        [Range(1, 1000000, ErrorMessage = "Please enter a valid number")]
        [Display(Name = "People Affected")]
        public int PeopleAffected { get; set; }

        [Display(Name = "Incident Date/Time")]
        [DataType(DataType.DateTime)]
        public DateTime? IncidentDate { get; set; }

        [Display(Name = "Upload Photo (Optional)")]
        public IFormFile ImageFile { get; set; }

        // Dropdown options
        public List<string> DisasterTypes => new List<string>
        {
            "Flood", "Earthquake", "Fire", "Storm", "Drought",
            "Conflict", "Disease Outbreak", "Other"
        };

        public List<string> SeverityLevels => new List<string>
        {
            "Low", "Medium", "High", "Critical"
        };

        public List<string> StatusOptions => new List<string>
        {
            "Reported", "Under Review", "Assistance Deployed", "Resolved"
        };
    }
}