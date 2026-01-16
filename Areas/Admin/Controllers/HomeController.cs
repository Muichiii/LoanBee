using LoanBee.Areas.Admin.Models;
using LoanBee.Data;
//using LoanBee.Models.ViewModels.Admin;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;


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

        // GET: /Admin/Home/Update/{id}
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var app = await _context.Applications
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Application_no == id);

            if (app == null) return NotFound();

            var vm = new AdminUpdateStatusVm
            {
                ApplicationNo = app.Application_no,
                CurrentStatus = app.Application_status,
                NewStatus = app.Application_status
            };

            return View(vm);
        }

        // POST: /Admin/Home/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(AdminUpdateStatusVm vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var app = await _context.Applications
                .FirstOrDefaultAsync(a => a.Application_no == vm.ApplicationNo);

            if (app == null) return NotFound();

            // Enforce allowed transitions
            if (!IsAllowedTransition(app.Application_status, vm.NewStatus))
            {
                ModelState.AddModelError("", "Invalid status transition.");
                vm.CurrentStatus = app.Application_status;
                return View(vm);
            }

            app.Application_status = vm.NewStatus;
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Home",
                new { area = "Admin", id = vm.ApplicationNo });
        }

        private static bool IsAllowedTransition(string current, string next)
        {
            if (current == next) return true;

            return current switch
            {
                "Under Review" => next == "Approved" || next == "Rejected",
                "Approved" => next == "Completed",
                _ => false
            };
        }

        // GET: /Admin/Home/Delete/{id}
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var app = await _context.Applications
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Application_no == id);

            if (app == null) return NotFound();

            if (!IsDeleteAllowed(app.Application_status))
                return Forbid(); // or return BadRequest("Not allowed");

            var vm = new AdminDeleteApplicationVm
            {
                ApplicationNo = app.Application_no,
                Status = app.Application_status,
                DateApplied = app.Application_date,
                Amount = app.Loan_amount,
                Tenor = app.Loan_tenor,
                Purpose = app.Loan_purpose
            };

            return View(vm);
        }

        // POST: /Admin/Home/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(AdminDeleteApplicationVm vm)
        {
            var app = await _context.Applications
                .FirstOrDefaultAsync(a => a.Application_no == vm.ApplicationNo);

            if (app == null) return NotFound();

            // Server-side enforcement
            if (!IsDeleteAllowed(app.Application_status))
                return Forbid();

            _context.Applications.Remove(app);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        private static bool IsDeleteAllowed(string status)
        {
            return status == "Rejected" || status == "Completed";
        }

        // FOR LOGOUT
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(userId))
            {
                context.Result = RedirectToAction("Login", "Account", new { area = "" });
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}