using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanBee.Models.Entities
{
    public class Application
    {
        [Key]
        public Guid Application_no { get; set; }

        // This is the Foreign Key property
        public string Owner_tin_no { get; set; } = null!;

        // This is the Navigation Property
        [ForeignKey("Owner_tin_no")]
        public virtual Owner? Owner { get; set; } = null!;

        [Column(TypeName = "date")]
        public DateTime Application_date { get; set; }

        public string Application_status { get; set; } = null!;

        public int Loan_amount { get; set; }

        [Required(ErrorMessage = "This field is required.")]
        public string Loan_purpose { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        public string Loan_tenor { get; set; } = null!;

        // FK to Business
        [Required]
        public string Business_tin_no { get; set; } = null!;

        [ForeignKey(nameof(Business_tin_no))]
        public Business Business { get; set; } = null!;

        // FK to BankAccount
        [Required]
        public string Account_no { get; set; } = null!;

        [ForeignKey(nameof(Account_no))]
        public BankAccount BankAccount { get; set; } = null!;
    }
}

