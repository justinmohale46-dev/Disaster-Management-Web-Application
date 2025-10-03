using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.ViewModels
{
    public class DonationViewModel
    {
        // Required fields
        [Required(ErrorMessage = "Please select what you'd like to donate")]
        public string ResourceType { get; set; }

        [Required(ErrorMessage = "Please specify how many items")]
        [Range(1, 10000)]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Please describe what you're donating")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please select the condition")]
        public string Condition { get; set; }

        [Required(ErrorMessage = "Please select delivery method")]
        public string DeliveryMethod { get; set; }

       

        // ADD THIS PROPERTY - Resource Types for dropdown
        public List<string> ResourceTypes => new List<string>
        {
            "Food & Water",
            "Clothing & Blankets",
            "Medical Supplies",
            "Hygiene Products",
            "Baby Items",
            "School Supplies",
            "Building Materials",
            "Other Essentials"
        };

        // ADD THESE TOO for other dropdowns
        public List<string> Conditions => new List<string>
        {
            "New",
            "Used but good condition",
            "Needs repair"
        };

        public List<string> DeliveryMethods => new List<string>
        {
            "I will drop off",
            "Please collect from me"
        };
        public List<string> TimeSlots => new List<string>
        {
            "Morning (8am - 12pm)",
            "Afternoon (12pm - 4pm)",
            "Evening (4pm - 6pm)"
        };
    }
}