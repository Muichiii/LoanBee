using System.ComponentModel.DataAnnotations;

namespace LoanBee.Models.Entities
{
    public class Owner
    {
        [Key]
        public string Owner_tin_no { get; set; } = null!;

        // other fields...
        public string Owner_name { get; set; } = null!;
        public string Owner_gender { get; set; } = null!;
        public DateTime Owner_birthday { get; set; }
        public string Owner_citizenship { get; set; } = null!;
        public string Owner_civil_status { get; set; } = null!;
        public string Owner_address { get; set; } = null!;
        public string Owner_place_of_birth { get; set; } = null!;
        public string Owner_mobile_no { get; set; } = null!;
        public string Owner_landline { get; set; } = null!;
        public string Owner_email_address { get; set; } = null!;
        public string Owner_education { get; set; } = null!;

        // Navigations
        public ICollection<Business> Businesses { get; set; } = new List<Business>();
        public ICollection<BankAccount> BankAccounts { get; set; } = new List<BankAccount>();
    }
}
