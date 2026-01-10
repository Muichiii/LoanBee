using System.ComponentModel.DataAnnotations;

namespace LoanBee.Models.Entities
{
    public class Application
    {
        [Key]
        public Guid application_no { get; set; }

        public int loan_amount { get; set; }
        public string loan_tenor { get; set; }
        public string loan_purpose { get; set; }
        public DateTime application_date { get; set; }
        public string application_status { get; set; }
    }
}

