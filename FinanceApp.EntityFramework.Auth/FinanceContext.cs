using FinanceApp.Shared;
using FinanceApp.Shared.Entities.CommonTables;
using FinanceApp.Shared.Entities.UserTables;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.EntityFramework
{
    public class UserContext : IdentityDbContext<CustomIdentityUser, IdentityRole<int>, int>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserContext(DbContextOptions<UserContext> opt, IHttpContextAccessor httpContextAccessor) : base(opt)
        {
            _httpContextAccessor = httpContextAccessor;
        } 

        //User Tables
        public DbSet<PrivateFixedIncome> PrivateFixedIncomes { get; set; }
        public DbSet<TreasuryBond> TreasuryBonds { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Spending> Spendings { get; set; }
        public DbSet<Income> Incomes { get; set; }
        public DbSet<FGTS> FGTS { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<CurrentBalance> CurrentBalances { get; set; }
        public DbSet<CreditCard> CreditCards { get; set; }
        public DbSet<ForecastParameters> ForecastParameters { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            CustomIdentityUser admin = new CustomIdentityUser
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