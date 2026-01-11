using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanBee.Models.Entities
{
    public class Application
    {
        [Key]
        public Guid Application_no { get; set; }

        [Column(TypeName = "date")]
        public DateTime Application_date { get; set; }

        public string Application_status { get; set; } = null!;

        public int Loan_amount { get; set; }
        public string Loan_purpose { get; set; } = null!;
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

