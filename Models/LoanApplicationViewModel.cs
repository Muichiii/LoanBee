using LoanBee.Models.Entities;

namespace LoanBee.Models
{
    public class LoanApplicationViewModel
    {
        public Application Application { get; set; } = new Application();
        public Business Business { get; set; } = new Business();
        public Owner Owner { get; set; } = new Owner();
        public BankAccount BankAccount { get; set; } = new BankAccount();
    }
}