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
        public async Task<IActionResult> CreateLoanApplication(LoanApplicationViewModel model)
        {
            if (ModelState.IsValid)
            {

                model.Application.Application_no = Guid.NewGuid();
                model.Application.Application_date = DateTime.Now;
                model.Application.Application_status = "Pending";

                _context.Applications.Add(model.Application);
                _context.Businesses.Add(model.Business);
                _context.Owners.Add(model.Owner);
                _context.BankAccounts.Add(model.BankAccount);

                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
                }
            }

            return View(model);
        }
    }
}