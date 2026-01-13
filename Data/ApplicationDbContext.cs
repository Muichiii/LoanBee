using Microsoft.EntityFrameworkCore;
using LoanBee.Models.Entities;

namespace LoanBee.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Business> Businesses { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Owner> Owners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Link Owner to User
            modelBuilder.Entity<Application>()
                .HasOne(a => a.Owner)           // Application has one Owner
                .WithMany(o => o.Applications)  // Owner has many Applications
                .HasForeignKey(a => a.Owner_tin_no) // Use this specific column
                .IsRequired();

            // Business -> Owner (many businesses can belong to one owner)
            modelBuilder.Entity<Business>()
                .HasOne(b => b.Owner)
                .WithMany(o => o.Businesses)
                .HasForeignKey(b => b.Owner_tin_no)
                .OnDelete(DeleteBehavior.Restrict);

            // BankAccount -> Owner (many accounts can belong to one owner)
            modelBuilder.Entity<BankAccount>()
                .HasOne(a => a.Owner)
                .WithMany(o => o.BankAccounts)
                .HasForeignKey(a => a.Owner_tin_no)
                .OnDelete(DeleteBehavior.Restrict);

            // Application -> Business
            modelBuilder.Entity<Application>()
                .HasOne(a => a.Business)
                .WithMany() // no navigation collection on Business currently
                .HasForeignKey(a => a.Business_tin_no)
                .OnDelete(DeleteBehavior.Restrict);

            // Application -> BankAccount
            modelBuilder.Entity<Application>()
                .HasOne(a => a.BankAccount)
                .WithMany() // no navigation collection on BankAccount currently
                .HasForeignKey(a => a.Account_no)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
