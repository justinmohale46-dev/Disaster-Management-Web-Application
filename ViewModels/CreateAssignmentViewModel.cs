using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.ViewModels
{
    public class CreateAssignmentViewModel
    {
        [Required]
        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Required]
        [Display(Name = "Project Description")]
        public string ProjectDescription { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }
    }
}
