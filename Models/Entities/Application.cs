using System.ComponentModel.DataAnnotations;

namespace LoanBee.Models.Entities
{
    public class Application
    {
        [Key]
        public Guid Application_no { get; set; }

        public int Loan_amount { get; set; }
        public string Loan_tenor { get; set; }
        public string Loan_purpose { get; set; }
        public DateTime Application_date { get; set; }
        public string Application_status { get; set; }
    }
}

