using System.ComponentModel.DataAnnotations;

namespace LoanBee.Areas.Admin.Models
{
    public class AdminUpdateStatusVm
    {
        [Required]
        public Guid ApplicationNo { get; set; }

        public string CurrentStatus { get; set; } = "";

        [Required]
        public string NewStatus { get; set; } = "";
    }
}
