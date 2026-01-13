using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanBee.Models.Entities
{
    public class Owner
    {
        [Key]
        [Required(ErrorMessage = "This field is required.")]
        public string Owner_tin_no { get; set; } = null!;

        // FK to User
        public Guid UserId { get; set; } 
        public User User { get; set; } = null!; 

        // other fields...
        [Required(ErrorMessage = "This field is required.")]
        public string Owner_name { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        public string Owner_gender { get; set; } = null!;

        [Column(TypeName = "date")]
        public DateTime Owner_birthday { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string Owner_citizenship { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        public string Owner_civil_status { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        public string Owner_address { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        public string Owner_place_of_birth { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        public string Owner_mobile_no { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        public string Owner_landline { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        public string Owner_email_address { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        public string Owner_education { get; set; } = null!;

        // Navigations
        public ICollection<Business> Businesses { get; set; } = new List<Business>();
        public ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
        public ICollection<Application> Applications { get; set; } = new List<Application>();
    }
}
