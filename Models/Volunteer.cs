using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftOfTheGivers.Models
{
    public class Volunteer
    {
        [Key]
        public int VolunteerID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; }

        // Contact Information
        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(100)]
        public string EmergencyContact { get; set; }

        [Required]
        [StringLength(20)]
        public string EmergencyContactPhone { get; set; }

        // Skills & Experience
        [Required]
        public string Skills { get; set; } // Comma-separated: "Medical,Logistics,Driving"

        public string PreviousExperience { get; set; }

        // Availability
        [Required]
        public string Availability { get; set; } // "Weekdays,Weekends,Both"

        [Required]
        public string PreferredTasks { get; set; } // "Distribution,Medical,Construction,Admin"

        // Status
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Active";

        [DataType(DataType.DateTime)]
        public DateTime RegistrationDate { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}