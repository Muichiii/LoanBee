using Microsoft.AspNetCore.Mvc;
using LoanBee.Models.Entities;
using LoanBee.Data;
using Microsoft.AspNetCore.Http;

namespace LoanBee.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Signup(User user)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }
            return View("Login", user);
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                // Store user info in Session
                HttpContext.Session.SetString("UserName", user.Name);
                HttpContext.Session.SetString("UserId", user.Id.ToString());

                // Check if the user is the admin
                if (user.Email == "admin@gmail.com" && user.Password == "admin123")
                {
                    // Redirect to the Index action of the HomeController inside the Admin Area
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }

                // Default redirect for regular users
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Invalid email or password";
            return View();
        }
    }
}