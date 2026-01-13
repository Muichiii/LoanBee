using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LoanBee.Models.Entities
{
    public class Business
    {
        [Key]
        public string Business_tin_no { get; set; } = null!;

        // FK to Owner
        [Required(ErrorMessage = "This field is required.")]
        public string Owner_tin_no { get; set; } = null!;

        [ForeignKey(nameof(Owner_tin_no))]
        public Owner Owner { get; set; } = null!;

        // other fields...
        [Required(ErrorMessage = "This field is required.")]
        public string Business_name { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        public string Business_type { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        public string Office_address { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        public string Office_zip { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        public string Business_landline { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        public string Business_mobile_no { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        public string Business_email { get; set; } = null!;

        [Required(ErrorMessage = "This field is required.")]
        public string Business_website { get; set; } = null!;
    }
}
