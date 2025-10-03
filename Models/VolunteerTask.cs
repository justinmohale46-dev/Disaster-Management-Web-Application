using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftOfTheGivers.Models
{
    public class VolunteerTask
    {
        [Key]
        public int TaskID { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [StringLength(100)]
        public string Category { get; set; } // "Distribution", "Medical", "Logistics", "Construction", "Admin"

        [Required]
        [StringLength(255)]
        public string Location { get; set; }

        [Required]
        public string RequiredSkills { get; set; } // Comma-separated

        [DataType(DataType.DateTime)]
        public DateTime TaskDate { get; set; }

        public int DurationHours { get; set; }

        public int VolunteersNeeded { get; set; }

        public int VolunteersAssigned { get; set; } = 0;

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Open"; // Open, Filled, Completed, Cancelled

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public int? ReportID { get; set; }

        [ForeignKey("ReportID")]
        public DisasterReport DisasterReport { get; set; }


    }
}