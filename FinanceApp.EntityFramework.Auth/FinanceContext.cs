using FinanceApp.Shared.Entities.CommonTables;
using FinanceApp.Shared.Entities.UserTables;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.EntityFramework
{
    public class UserContext : IdentityDbContext<CustomIdentityUser, IdentityRole<int>, int>
    {
        public UserContext(DbContextOptions<UserContext> opt) : base(opt)
        {
            
        } 
        public DbSet<PrivateFixedIncome> PrivateFixedIncomes => Set<PrivateFixedIncome>();
        public DbSet<TreasuryBond> TreasuryBonds => Set<TreasuryBond>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Spending> Spendings => Set<Spending>();
        public DbSet<Income> Incomes => Set<Income>();
        public DbSet<FGTS> FGTS => Set<FGTS>();
        public DbSet<Loan> Loans => Set<Loan>();
        public DbSet<CurrentBalance> CurrentBalances => Set<CurrentBalance>();
        public DbSet<CreditCard> CreditCards => Set<CreditCard>();
        public DbSet<ForecastParameters> ForecastParameters => Set<ForecastParameters>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            CustomIdentityUser admin = new()
            {
                UserName = "admin",

                NormalizedUserName = "ADMIN",
                Email = "admin@admin.com",
                NormalizedEmail = "ADMIN@ADMIN.COM",
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString(),
                Id = 1
            };

            PasswordHasher<CustomIdentityUser> hasher = new();

            admin.PasswordHash = hasher.HashPassword(admin, "teste");

            builder.Entity<CustomIdentityUser>().HasData(admin);
                
            builder.Entity<Category>()
                 .HasIndex(p => new { p.Name })
                 .IsUnique(true);

            builder.Entity<FGTS>()
                 .HasIndex(p => new { p.UserId })
                 .IsUnique(true);

            builder.Entity<CurrentBalance>()
                 .HasIndex(p => new { p.UserId })
                 .IsUnique(true);

        }

    }
}