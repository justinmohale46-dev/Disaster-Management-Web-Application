using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftOfTheGivers.Models
{
    public class TaskApplication
    {
        [Key]
        public int ApplicationID { get; set; }

        [ForeignKey("Volunteer")]
        public int VolunteerID { get; set; }
        public Volunteer Volunteer { get; set; }

        [ForeignKey("VolunteerTask")]
        public int TaskID { get; set; }
        public VolunteerTask VolunteerTask { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Applied"; // Applied, Approved, Rejected, Completed

        public string ApplicationNotes { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime AppliedDate { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime? ApprovedDate { get; set; }
    }
}
