using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.ViewModels
{
    public class VolunteerAssignmentSelectionViewModel
    {
        public int AssignmentID { get; set; }
        public string ProjectName { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
    }
}
