using Microsoft.AspNetCore.Mvc;
using LoanBee.Data;
using LoanBee.Models;
using LoanBee.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoanBee.Controllers
{
    public class LoanBeeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LoanBeeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult CreateLoanApplication()
        {
            return View(new LoanApplicationViewModel());
        }

        // POST: /LoanBee/CreateLoanApplication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLoanApplication(LoanApplicationViewModel model, string submitButton)
        {
            // 1. Handle "Edit" button from Review Page
            if (submitButton == "Edit")
            {
                return View(model);
            }

            // 2. Clear validation for auto-generated fields
            ModelState.Remove("Application.Application_status");
            ModelState.Remove("Application.Business_tin_no");
            ModelState.Remove("Application.Account_no");
            ModelState.Remove("Business.Owner_tin_no");
            ModelState.Remove("BankAccount.Owner_tin_no");
            ModelState.Remove("Application.Business");
            ModelState.Remove("Application.BankAccount");
            ModelState.Remove("Business.Owner");
            ModelState.Remove("BankAccount.Owner");
            ModelState.Remove("Owner.User");

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // 3. Handle "Next" button (Redirect to Review Page)
            if (submitButton == "Next")
            {
                ViewBag.IsPreview = true;
                return View(model);
            }

            // 4. Handle "Submit" button (Final Database Save)
            if (submitButton == "Submit")
            {
                // Verify User Session
                var userIdString = HttpContext.Session.GetString("UserId");
                if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
                {
                    return RedirectToAction("Login", "Account");
                }

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // STAGE A: Save Owner first (Parent of Business and Bank)
                        model.Owner.UserId = userId;
                        _context.Owners.Add(model.Owner);
                        await _context.SaveChangesAsync();

                        // STAGE B: Link and Save Business & Bank Account
                        model.Business.Owner_tin_no = model.Owner.Owner_tin_no;
                        model.BankAccount.Owner_tin_no = model.Owner.Owner_tin_no;

                        _context.Businesses.Add(model.Business);
                        _context.BankAccounts.Add(model.BankAccount);
                        await _context.SaveChangesAsync();

                        // STAGE C: Finalize and Save Application
                        model.Application.Application_no = Guid.NewGuid();
                        model.Application.Application_date = DateTime.Now;
                        model.Application.Application_status = "Under Review";
                        model.Application.Business_tin_no = model.Business.Business_tin_no;
                        model.Application.Account_no = model.BankAccount.Account_no;

                        _context.Applications.Add(model.Application);
                        await _context.SaveChangesAsync();

                        // Commit all changes if everything succeeded
                        await transaction.CommitAsync();

                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception ex)
                    {
                        // If anything fails, roll back the whole process
                        await transaction.RollbackAsync();
                        ModelState.AddModelError("", "An error occurred while saving: " + ex.Message);
                        ViewBag.IsPreview = true;
                        return View(model);
                    }
                }
            }

            return View(model);
        }

        // GET: /LoanBee/ReadLoanApplication
        [HttpGet]
        public IActionResult ReadLoanApplication(string ownerTin)
        {
            if (string.IsNullOrEmpty(ownerTin))
            {
                return View(new List<Application>());
            }

            // Filter applications based on Owner TIN via the Business relationship
            var applications = _context.Applications
                .Include(a => a.Business)
                .Where(a => a.Business.Owner_tin_no == ownerTin)
                .ToList();

            ViewBag.OwnerTin = ownerTin;
            return View(applications);
        }

        // GET: /LoanBee/ReadLoanApplicationDetails
        public async Task<IActionResult> ReadLoanApplicationDetails(Guid id)
        {
            // Eager load all related entities for the detailed view
            var application = await _context.Applications
                .Include(a => a.Business)
                .Include(a => a.BankAccount)
                .Include(a => a.Business.Owner)
                .FirstOrDefaultAsync(a => a.Application_no == id);

            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }


    }
}