using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.ViewModels
{
    public class DonationItemViewModel
    {
        public int DonationID { get; set; }

        [Display(Name = "Donation Date")]
        public DateTime DonationDate { get; set; }

        [Display(Name = "Resource Type")]
        public string ResourceType { get; set; }

        public int Quantity { get; set; }

        [Display(Name = "Item Description")]
        public string Description { get; set; }

        public string Condition { get; set; }

        [Display(Name = "Delivery Method")]
        public string DeliveryMethod { get; set; }

        public string Status { get; set; }

       
    }
}
