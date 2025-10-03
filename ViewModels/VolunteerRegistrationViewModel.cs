using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.ViewModels
{
    public class VolunteerRegistrationViewModel
    {



        // Contact Information
        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Emergency contact is required")]
        [Display(Name = "Emergency Contact Name")]
        public string EmergencyContact { get; set; }

        [Required(ErrorMessage = "Emergency contact phone is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Emergency Contact Phone")]
        public string EmergencyContactPhone { get; set; }

        // Skills & Experience
        [Required(ErrorMessage = "Please select at least one skill")]
        [Display(Name = "Your Skills")]
        public List<string> SelectedSkills { get; set; } = new List<string>();

        [Display(Name = "Previous Volunteer Experience")]
        public string PreviousExperience { get; set; }

        // Availability
        [Required(ErrorMessage = "Please select your availability")]
        [Display(Name = "When are you available?")]
        public string Availability { get; set; }

        [Required(ErrorMessage = "Please select preferred task types")]
        [Display(Name = "Preferred Task Types")]
        public List<string> SelectedTaskTypes { get; set; } = new List<string>();

        // Dropdown options (same as before)
        public List<string> SkillOptions => new List<string>
        {
            "Medical", "First Aid", "Nursing", "Doctor",
            "Logistics", "Driving", "Warehouse", "Distribution",
            "Construction", "Electrical", "Plumbing", "Carpentry",
            "Administration", "IT Support", "Communication", "Cooking",
            "Teaching", "Counseling", "Language Translation", "Other"
        };

        public List<string> AvailabilityOptions => new List<string>
        {
            "Weekdays Only", "Weekends Only", "Both Weekdays and Weekends",
            "Flexible", "Emergency Response Only"
        };

        public List<string> TaskTypeOptions => new List<string>
        {
            "Food Distribution", "Medical Assistance", "Shelter Construction",
            "Logistics Support", "Administrative Work", "Community Outreach",
            "Emergency Response", "Fundraising", "Other"
        };
    }
}