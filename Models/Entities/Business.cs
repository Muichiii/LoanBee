using System.ComponentModel.DataAnnotations;

namespace LoanBee.Models.Entities
{
    public class Business
    {
        [Key]
        public string Business_tin_no { get; set; }

        public string Business_name { get; set; }
        public string Business_type { get; set; }
        public string Office_address { get; set; }
        public string Office_zip { get; set; }
        public string Business_landline { get; set; }
        public string Business_mobile_no { get; set; }
        public string Business_email { get; set; }
        public string Business_website { get; set; }
    }
}
