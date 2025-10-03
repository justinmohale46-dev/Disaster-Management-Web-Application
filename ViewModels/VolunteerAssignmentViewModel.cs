using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.ViewModels
{
    public class VolunteerAssignmentViewModel
    {
        public int AssignmentID { get; set; }

        [Required(ErrorMessage = "Please select a volunteer.")]
        public int UserID { get; set; } // Volunteer ID

        [Display(Name = "Volunteer Name")]
        public string VolunteerName { get; set; }

        [Required(ErrorMessage = "Please select a project.")]
        public int ProjectID { get; set; } // Relief project ID

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Required(ErrorMessage = "Please enter the volunteer's role.")]
        [StringLength(50, ErrorMessage = "Role can't exceed 50 characters.")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Please select a status.")]
        [StringLength(50, ErrorMessage = "Status can't exceed 50 characters.")]
        public string Status { get; set; }

        [Display(Name = "Assigned Date")]
        [DataType(DataType.DateTime)]
        public DateTime AssignedDate { get; set; } = DateTime.Now;
    }
}
