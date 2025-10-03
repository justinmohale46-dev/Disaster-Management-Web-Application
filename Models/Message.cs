using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftOfTheGivers.Models
{
    public class Message
    {
        [Key]
        public int MessageID { get; set; }

        [ForeignKey("User")]
        public int? FromUserID { get; set; } // Null for admin broadcasts
        public User FromUser { get; set; }

        public string Subject { get; set; }

        [Required]
        public string Content { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime SentDate { get; set; } = DateTime.Now;

        public string MessageType { get; set; } // "AdminBroadcast", "VolunteerToAdmin"
    }
}