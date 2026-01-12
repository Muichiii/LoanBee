using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoanBee.Data;
namespace LoanBee.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // DASHBOARD - SEARCH IN TABLE
        // /Admin/Home/Index?query=...&status=...
        [HttpGet]
        public async Task<IActionResult> Index(string? query, string? status)
        {
            query = query?.Trim();
            status = status?.Trim();

            // JOIN approach (works even if you don't have navigation properties)
            var q = from a in _context.Applications.AsNoTracking()
                    join b in _context.Businesses.AsNoTracking()
                        on a.Business_tin_no equals b.Business_tin_no
                    join o in _context.Owners.AsNoTracking()
                        on b.Owner_tin_no equals o.Owner_tin_no
                    join ba in _context.BankAccounts.AsNoTracking()
                        on a.Account_no equals ba.Account_no
                    select new AdminLoanApplicationRowVm
                    {
                        ApplicationNo = a.Application_no,
                        ApplicantName = o.Owner_name,
                        Amount = a.Loan_amount,
                        Tenor = a.Loan_tenor,
                        Purpose = a.Loan_purpose,
                        DateApplied = a.Application_date,
                        Status = a.Application_status
                    };

            // Filter by search query
            if (!string.IsNullOrWhiteSpace(query))
            {
                q = q.Where(x =>
                    x.ApplicantName.Contains(query) ||
                    x.Purpose.Contains(query) ||
                    x.Tenor.Contains(query) ||
                    x.Status.Contains(query) ||
                    x.ApplicationNo.ToString().Contains(query)
                );
            }

            // Filter by status
            if (!string.IsNullOrWhiteSpace(status))
            {
                q = q.Where(x => x.Status == status);
            }

            var vm = new AdminLoanApplicationsVm
            {
                Query = query ?? "",
                Status = status ?? "",
                Rows = await q.OrderByDescending(x => x.DateApplied).Take(200).ToListAsync()
            };

            return View(vm);
        }
    }

    public class AdminLoanApplicationsVm
    {
        public string Query { get; set; } = "";
        public string Status { get; set; } = "";
        public List<AdminLoanApplicationRowVm> Rows { get; set; } = new();
    }

    public class AdminLoanApplicationRowVm
    {
        public Guid ApplicationNo { get; set; }
        public string ApplicantName { get; set; } = "";
        public int Amount { get; set; }
        public string Tenor { get; set; } = "";
        public string Purpose { get; set; } = "";
        public DateTime DateApplied { get; set; }
        public string Status { get; set; } = "";
    }
}