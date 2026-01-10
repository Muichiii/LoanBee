using System.ComponentModel.DataAnnotations;

namespace LoanBee.Models.Entities
{
    public class Owner
    {
        [Key]
        public string Owner_tin_no { get; set; }

        public string Owner_name { get; set; }
        public string Owner_gender { get; set; }
        public DateTime Owner_birthday { get; set; }
        public string Owner_address { get; set; }
        public string Owner_place_of_birth { get; set; }
        public string Owner_citizenship { get; set; }
        public string Owner_civil_status { get; set; }
        public string Owner_mobile_no { get; set; }
        public string Owner_landline { get; set; }
        public string Owner_email_address { get; set; }
        public string Owner_education { get; set; }
    }
}
