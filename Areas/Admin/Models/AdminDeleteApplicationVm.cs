namespace LoanBee.Areas.Admin.Models
{
    public class AdminDeleteApplicationVm
    {
        public Guid ApplicationNo { get; set; }
        public string Status { get; set; } = "";
        public DateTime DateApplied { get; set; }
        public int Amount { get; set; }
        public string Tenor { get; set; } = "";
        public string Purpose { get; set; } = "";
    }
}
