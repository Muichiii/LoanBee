namespace LoanBee.Areas.Admin.Models
{
    public class AdminLoanApplicationsVm
    {
        public string Query { get; set; } = "";
        public string Status { get; set; } = "";
        public List<AdminLoanApplicationRowVm> Rows { get; set; } = new();

    }
}
