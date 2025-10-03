using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftOfTheGivers.Models
{
    public class Donation
    {
        [Key]
        public int DonationID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; }

        // RESOURCE DONATION (Primary focus)
        [Required(ErrorMessage = "Please select a resource type")]
        [StringLength(100)]
        public string ResourceType { get; set; }

        [Required(ErrorMessage = "Please specify the quantity")]
        [Range(1, 10000, ErrorMessage = "Quantity must be between 1 and 10,000")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "Please describe the items")]
        [StringLength(500)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please select the condition")]
        [StringLength(50)]
        public string Condition { get; set; } // New, Used but good, Needs repair

        [Required(ErrorMessage = "Please select delivery method")]
        [StringLength(50)]
        public string DeliveryMethod { get; set; } // I will drop off, Please collect from me

        
        

        [DataType(DataType.DateTime)]
        public DateTime DonationDate { get; set; } = DateTime.Now;

        [Required, StringLength(50)]
        public string Status { get; set; } = "Pending";
    }
}