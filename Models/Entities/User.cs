using System.ComponentModel.DataAnnotations;

namespace LoanBee.Models.Entities
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "This field is required.")]
        public string Name { get; set; } = null!;
        [Required(ErrorMessage = "This field is required.")]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required(ErrorMessage = "This field is required.")]
        public string Password { get; set; } = null!;
    }
}