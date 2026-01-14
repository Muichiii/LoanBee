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
        public async Task<IActionResult> CreateLoanApplication()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // Check if the user already has an Owner profile
            var existingOwner = await _context.Owners
                .FirstOrDefaultAsync(o => o.UserId == userId);

            var viewModel = new LoanApplicationViewModel();

            if (existingOwner != null)
            {
                // Auto-fill the form with existing data
                viewModel.Owner = existingOwner;
            }

            return View(viewModel);
        }

        // POST: /LoanBee/CreateLoanApplication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLoanApplication(LoanApplicationViewModel model, string submitButton)
        {
            // 1. NUCLEAR CLEANUP: Remove all system-generated and navigation properties
            // This stops "Application_status", "Owner", and "Owner_tin_no" errors

            // Application Structural Fields
            ModelState.Remove("Application.Application_status");
            ModelState.Remove("Application.Application_no");
            ModelState.Remove("Application.Application_date");
            ModelState.Remove("Application.Owner_tin_no");
            ModelState.Remove("Application.Business_tin_no");
            ModelState.Remove("Application.Account_no");

            // Navigation Objects (The "Owner" and "Business" links)
            ModelState.Remove("Application.Owner");
            ModelState.Remove("Application.Business");
            ModelState.Remove("Application.BankAccount");
            ModelState.Remove("Owner"); // The root Owner object in ViewModel

            // Owner Internal Links
            ModelState.Remove("Owner.UserId");
            ModelState.Remove("Owner.User");
            ModelState.Remove("Owner.Applications");
            ModelState.Remove("Owner.Businesses");
            ModelState.Remove("Owner.BankAccounts");

            // Business & Bank Internal Links
            ModelState.Remove("Business.Owner");
            ModelState.Remove("BankAccount.Owner");
            ModelState.Remove("Business.Owner_tin_no");
            ModelState.Remove("BankAccount.Owner_tin_no");

            // 3. Now check if the actual user-inputted data is valid
            if (!ModelState.IsValid)
            {
                // If errors still appear, they will be about actual fields like Name or Amount
                ViewBag.IsPreview = false;
                return View(model);
            }

            if (submitButton == "Next")
            {
                ViewBag.IsPreview = true;
                return View(model);
            }

            // 5. Handle "Submit" button (Final Database Save)
            if (submitButton == "Submit")
            {
                // 1. Verify User Session
                var userIdString = HttpContext.Session.GetString("UserId");
                if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
                {
                    return RedirectToAction("Login", "Account");
                }

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        // 2. Handle Owner Information (Create or Update)
                        // Check if this TIN already exists in the database
                        var existingOwner = await _context.Owners
                            .AsNoTracking()
                            .FirstOrDefaultAsync(o => o.Owner_tin_no == model.Owner.Owner_tin_no);

                        if (existingOwner != null)
                        {
                            // If owner exists, update their profile with the latest info from the form
                            model.Owner.UserId = userId; // Keep the owner linked to this account
                            _context.Owners.Update(model.Owner);
                        }
                        else
                        {
                            // If new owner, assign the UserId and Add
                            model.Owner.UserId = userId;
                            _context.Owners.Add(model.Owner);
                        }

                        // Save immediately so the TIN is registered for the next steps
                        await _context.SaveChangesAsync();

                        // 3. Link and Save Business
                        // Since an owner can have multiple businesses, we check if this specific TIN exists
                        var existingBusiness = await _context.Businesses
                            .AsNoTracking()
                            .FirstOrDefaultAsync(b => b.Business_tin_no == model.Business.Business_tin_no);

                        model.Business.Owner_tin_no = model.Owner.Owner_tin_no;
                        if (existingBusiness != null)
                            _context.Businesses.Update(model.Business);
                        else
                            _context.Businesses.Add(model.Business);

                        // 4. Link and Save Bank Account
                        var existingBank = await _context.BankAccounts
                            .AsNoTracking()
                            .FirstOrDefaultAsync(b => b.Account_no == model.BankAccount.Account_no);

                        model.BankAccount.Owner_tin_no = model.Owner.Owner_tin_no;
                        if (existingBank != null)
                            _context.BankAccounts.Update(model.BankAccount);
                        else
                            _context.BankAccounts.Add(model.BankAccount);

                        await _context.SaveChangesAsync();

                        // 5. Create and Save the New Application
                        // Applications are always new (Guid.NewGuid()) even for existing owners
                        model.Application.Application_no = Guid.NewGuid();
                        model.Application.Application_date = DateTime.Now;
                        model.Application.Application_status = "Under Review";

                        // Critical Foreign Key Links
                        model.Application.Owner_tin_no = model.Owner.Owner_tin_no;
                        model.Application.Business_tin_no = model.Business.Business_tin_no;
                        model.Application.Account_no = model.BankAccount.Account_no;

                        _context.Applications.Add(model.Application);
                        await _context.SaveChangesAsync();

                        // 6. Commit everything to the Database
                        await transaction.CommitAsync();

                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception ex)
                    {
                        // If any error occurs (e.g., DB connection, FK conflict), nothing is saved
                        await transaction.RollbackAsync();
                        ModelState.AddModelError("", "Submission failed: " + ex.Message);
                        ViewBag.IsPreview = true; // Stay on the review page so they can try again
                        return View(model);
                    }
                }
            }

            return View(model);
        }

        public async Task<IActionResult> ViewAllApplications(string status = "All")
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.CurrentFilter = status;

            var query = _context.Applications
                .Include(a => a.Owner)
                .Include(a => a.Business)
                .Where(a => a.Owner.UserId == userId);

            // Apply the filter (e.g., if status is "Rejected", it matches "Rejected" in DB)
            if (status != "All")
            {
                query = query.Where(a => a.Application_status == status);
            }

            var applications = await query.OrderByDescending(a => a.Application_date).ToListAsync();
            return View(applications);
        }

        public async Task<IActionResult> ApplicationDetails(Guid id)
        {
            // Eager load all related entities so the View has access to all properties
            var application = await _context.Applications
                .Include(a => a.Business)   // Loads Business info (Name, TIN, etc.)
                .Include(a => a.Owner)      // Loads Owner info (Name, Gender, etc.)
                .Include(a => a.BankAccount) // Loads Bank info (Type, Account No)
                .FirstOrDefaultAsync(a => a.Application_no == id);

            // If the ID doesn't exist in the database, return a 404
            if (application == null)
            {
                return NotFound();
            }

            // Pass the populated application object to your .cshtml view
            return View(application);
        }


    }
}