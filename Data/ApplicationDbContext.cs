using Microsoft.EntityFrameworkCore;
using LoanBee.Models.Entities;

namespace LoanBee.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }

        public DbSet<Application> Applications { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Owner> Owners { get; set; }

    }
}
