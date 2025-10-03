using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.Models
{


    public class User
{
    [Key]
    public int UserID { get; set; }

    [Required, StringLength(100)]
    public string FirstName { get; set; }

    [Required, StringLength(100)]
    public string LastName { get; set; }

    [Required, EmailAddress, StringLength(255)]
    public string Email { get; set; }

    [Required, StringLength(255)]
    public string PasswordHash { get; set; }

    [Required, StringLength(50)]
    public string Role { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [DataType(DataType.DateTime)]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [StringLength(20)]
    [Phone(ErrorMessage = "Invalid phone number format")]
     public string PhoneNumber { get; set; }
    }
}