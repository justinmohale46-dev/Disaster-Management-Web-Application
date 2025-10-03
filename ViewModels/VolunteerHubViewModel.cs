using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.ViewModels
{
    public class VolunteerHubViewModel
    {
        // Dashboard Stats
        public int TotalHours { get; set; }
        public int TasksCompleted { get; set; }
        public int UpcomingShifts { get; set; }
        public int PendingApplications { get; set; }

        // Available Tasks
        public List<VolunteerTaskViewModel> AvailableTasks { get; set; } = new List<VolunteerTaskViewModel>();

        // My Schedule
        public List<VolunteerTaskViewModel> MySchedule { get; set; } = new List<VolunteerTaskViewModel>();

        // Quick Apply (for modal/form)
        public int SelectedTaskID { get; set; }
        public string ApplicationNotes { get; set; }
    }

    public class VolunteerTaskViewModel
    {
        public int TaskID { get; set; }

        [Display(Name = "Task Title")]
        public string Title { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public string Location { get; set; }

        [Display(Name = "Required Skills")]
        public string RequiredSkills { get; set; }

        [Display(Name = "Task Date")]
        public DateTime TaskDate { get; set; }

        [Display(Name = "Duration (hours)")]
        public int DurationHours { get; set; }

        [Display(Name = "Volunteers Needed")]
        public int VolunteersNeeded { get; set; }

        [Display(Name = "Volunteers Assigned")]
        public int VolunteersAssigned { get; set; }

        public string Status { get; set; }

        // For applications
        public string ApplicationStatus { get; set; }
        public int ApplicationID { get; set; }
    }
}
