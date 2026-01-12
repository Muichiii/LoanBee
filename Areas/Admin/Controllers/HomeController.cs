using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LoanBee.Data;
using LoanBee.Areas.Admin.Models;


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

        // DASHBOARD - VIEW APPLICATION
        // GET: /Admin/Home/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var details = await (
                from a in _context.Applications.AsNoTracking()
                join b in _context.Businesses.AsNoTracking()
                    on a.Business_tin_no equals b.Business_tin_no
                join o in _context.Owners.AsNoTracking()
                    on b.Owner_tin_no equals o.Owner_tin_no
                join ba in _context.BankAccounts.AsNoTracking()
                    on a.Account_no equals ba.Account_no
                where a.Application_no == id
                select new AdminLoanApplicationDetailsVm
                {
                    ApplicationNo = a.Application_no,
                    DateApplied = a.Application_date,
                    Status = a.Application_status,
                    Amount = a.Loan_amount,
                    Tenor = a.Loan_tenor,
                    Purpose = a.Loan_purpose,

                    // Owner
                    OwnerTin = o.Owner_tin_no,
                    OwnerName = o.Owner_name,
                    OwnerGender = o.Owner_gender,
                    OwnerBirthday = o.Owner_birthday,
                    OwnerAddress = o.Owner_address,
                    OwnerPlaceOfBirth = o.Owner_place_of_birth,
                    OwnerCitizenship = o.Owner_citizenship,
                    OwnerCivilStatus = o.Owner_civil_status,
                    OwnerMobile = o.Owner_mobile_no,
                    OwnerLandline = o.Owner_landline,
                    OwnerEmail = o.Owner_email_address,
                    OwnerEducation = o.Owner_education,

                    // Business
                    BusinessTin = b.Business_tin_no,
                    BusinessName = b.Business_name,
                    BusinessType = b.Business_type,
                    OfficeAddress = b.Office_address,
                    OfficeZip = b.Office_zip,
                    BusinessLandline = b.Business_landline,
                    BusinessMobile = b.Business_mobile_no,
                    BusinessEmail = b.Business_email,
                    BusinessWebsite = b.Business_website,

                    // Bank Account
                    AccountNo = ba.Account_no,
                    AccountType = ba.Account_type,
                    RelationshipSince = ba.Relationship_since
                }
            ).FirstOrDefaultAsync();

            if (details == null) return NotFound();

            return View(details);
        }

    }
}