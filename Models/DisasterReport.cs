using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftOfTheGivers.Models
{
    public class DisasterReport
    {
        [Key]
        public int ReportID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; }

        [Required(ErrorMessage = "Location is required")]
        [StringLength(255)]
        public string Location { get; set; }

        [Required(ErrorMessage = "Please select disaster type")]
        [StringLength(100)]
        public string DisasterType { get; set; }

        [Required(ErrorMessage = "Please select severity level")]
        [StringLength(50)]
        public string SeverityLevel { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please estimate number of people affected")]
        [Range(1, 1000000, ErrorMessage = "Please enter a valid number")]
        public int PeopleAffected { get; set; }

        // Optional fields
        [StringLength(500)]
        public string ImageUrl { get; set; } // For storing photo paths

        [DataType(DataType.DateTime)]
        public DateTime? IncidentDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ReportDate { get; set; } = DateTime.Now;

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Reported";
    }
}