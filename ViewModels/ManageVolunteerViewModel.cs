namespace GiftOfTheGivers.ViewModels
{
    public class ManageVolunteerViewModel
    {
        public int AssignmentID { get; set; }
        public string VolunteerName { get; set; } // Name of the volunteer
        public string ProjectName { get; set; }   // Name of the relief project
        public string Role { get; set; }          // Volunteer role in the project
        public string Status { get; set; }        // Status of the assignment (e.g., Pending, Approved)
        public DateTime? AssignedDate { get; set; }  // Date when the assignment was created
    }
}
