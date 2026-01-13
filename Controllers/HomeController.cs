using Microsoft.AspNetCore.Mvc;
using LoanBee.Data; 
using Microsoft.EntityFrameworkCore;
using LoanBee.Models.Entities;
using LoanBee.Models;
using System.Diagnostics;

namespace LoanBee.Controllers
{
    public class HomeController : Controller
    {
        // 1. Declare the database context field
        private readonly ApplicationDbContext _context;

        // 2. Inject the context through the constructor
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out Guid userId))
            {
                return RedirectToAction("Login", "Account");
            }

            // 3. Now you can use _context
            var owner = await _context.Owners
                .Include(o => o.BankAccounts)
                .Include(o => o.Applications)
                .FirstOrDefaultAsync(o => o.UserId == userId);

            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            return View(owner);
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
