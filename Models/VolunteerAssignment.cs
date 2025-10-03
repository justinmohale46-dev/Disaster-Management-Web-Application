using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftOfTheGivers.Models
{
    public class VolunteerAssignment
    {
        [Key]
        public int AssignmentID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        public User User { get; set; } // Assuming you have a User model defined

        [ForeignKey("ReliefProject")]
        public int ProjectID { get; set; }

        public ReliefProject ReliefProject { get; set; }

        [Required, StringLength(50)]
        public string Role { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime AssignedDate { get; set; } = DateTime.Now;

        [Required, StringLength(50)]
        public string Status { get; set; }

       
    }

}
