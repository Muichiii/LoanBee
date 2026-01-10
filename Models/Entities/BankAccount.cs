using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanBee.Models.Entities
{
    public class BankAccount
    {
        [Key]
        public string Account_no { get; set; } = null!;

        // FK to Owner
        [Required]
        public string Owner_tin_no { get; set; } = null!;

        [ForeignKey(nameof(Owner_tin_no))]
        public Owner Owner { get; set; } = null!;

        // other fields...
        public string Account_type { get; set; } = null!;
        public string Relationship_since { get; set; } = null!;
    }
}
