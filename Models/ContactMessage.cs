using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Models
{
    public class ContactMessage
    {
        [Key]
        public int MessageID { get; set; }

        [ForeignKey("User")]
        public int? UserID { get; set; }

        public User User { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required, EmailAddress, StringLength(255)]
        public string Email { get; set; }

        [Required]
        public string Message { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime SentDate { get; set; } = DateTime.Now;

        [Required, StringLength(50)]
        public string Status { get; set; }
    }
}
