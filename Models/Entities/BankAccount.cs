using System.ComponentModel.DataAnnotations;

namespace LoanBee.Models.Entities
{
    public class BankAccount
    {
        [Key]
        public string Account_no { get; set; }

        public string Account_type { get; set; }
        public string Relationship_since { get; set; }
    }
}
