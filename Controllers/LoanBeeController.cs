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

        //// POST: /LoanBee/CreateLoanApplication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLoanApplication(LoanApplicationViewModel model)
        {
            // Remove fields users don't input (system fields / FK fields / navigation props)
            ModelState.Remove("Application.Application_status");
            ModelState.Remove("Application.Business_tin_no");
            ModelState.Remove("Application.Account_no");
            ModelState.Remove("Business.Owner_tin_no");
            ModelState.Remove("BankAccount.Owner_tin_no");

            ModelState.Remove("Application.Business");
            ModelState.Remove("Application.BankAccount");
            ModelState.Remove("Business.Owner");
            ModelState.Remove("BankAccount.Owner");

            if (!ModelState.IsValid)
                return View(model);

            // System fields
            model.Application.Application_no = Guid.NewGuid();
            model.Application.Application_date = DateTime.Now;
            model.Application.Application_status = "Pending";

            // Foreign keys based on your rules
            model.Business.Owner_tin_no = model.Owner.Owner_tin_no;
            model.BankAccount.Owner_tin_no = model.Owner.Owner_tin_no;

            model.Application.Business_tin_no = model.Business.Business_tin_no;
            model.Application.Account_no = model.BankAccount.Account_no;

            // Add and save
            _context.Owners.Add(model.Owner);
            _context.Businesses.Add(model.Business);
            _context.BankAccounts.Add(model.BankAccount);
            _context.Applications.Add(model.Application);

            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            catch (DbUpdateException ex)
            {
                var msg = ex.InnerException?.Message ?? ex.Message;
                ModelState.AddModelError("", msg);
                return View(model);
            }
        }


    }
}