﻿using FinanceApp.Shared.Models;
using FinancialAPI.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UsuariosApi.Models;

namespace FinanceApp.EntityFramework.Auth
{
    public class FinanceContext : IdentityDbContext<CustomIdentityUser, IdentityRole<int>, int>
    {

        public FinanceContext(DbContextOptions<FinanceContext> opt) : base(opt)
        {
        }
        public FinanceContext()
        {
        }

        //User Tables
        public DbSet<PrivateFixedIncome> PrivateFixedIncomes { get; set; }
        public DbSet<TreasuryBond> TreasuryBonds { get; set; }
        public DbSet<IncomeCategory> IncomeCategories { get; set; }
        public DbSet<SpendingCategory> SpendingCategories { get; set; }
        public DbSet<Spending> Spendings { get; set; }
        public DbSet<Income> Incomes { get; set; }

        //Common Tables
        public DbSet<IndexValue> IndexValues { get; set; }
        public DbSet<TreasuryBondValue> TreasuryBondValues { get; set; }
        public DbSet<ProspectIndexValue> ProspectIndexValues { get; set; }
        public DbSet<Holiday> Holidays { get; set; }
        public DbSet<WorkingDaysByYear> WorkingDaysByYear { get; set; }
        public DbSet<TreasuryBondTitle> TreasuryBondTitles { get; set; }

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
                Id = 99999
            };

            PasswordHasher<CustomIdentityUser> hasher = new();

            admin.PasswordHash = hasher.HashPassword(admin,"teste");

            builder.Entity<CustomIdentityUser>().HasData(admin);

            builder.Entity<IdentityRole<int>>().HasData(
                new IdentityRole<int> { Id = 99999, Name = "admin", NormalizedName = "ADMIN" }
            );

            builder.Entity<IdentityRole<int>>().HasData(
                new IdentityRole<int> { Id = 99997, Name = "regular", NormalizedName = "REGULAR" }
            );

            builder.Entity<IdentityUserRole<int>>().HasData(
                new IdentityUserRole<int> { RoleId = 99999, UserId = 99999 }
                );

            //Common Tables
            builder.Entity<IndexValue>()
              .HasIndex(p => new { p.Date, p.DateEnd, p.Index })
              .IsUnique(true);

            builder.Entity<ProspectIndexValue>()
               .HasIndex(p => new { p.DateStart, p.DateEnd, p.Index, p.BaseCalculo })
               .IsUnique(true);


            builder.Entity<WorkingDaysByYear>()
                 .HasIndex(p => new { p.Year })
                 .IsUnique(true);


            builder.Entity<Holiday>()
                 .HasIndex(p => new { p.Date })
                 .IsUnique(true);

            builder.Entity<TreasuryBondTitle>()
                .HasIndex(p => new { p.ExpirationDate, p.Type })
                .IsUnique(true);

            builder.Entity<TreasuryBondValue>()
                 .HasIndex(p => new { p.Date, p.ExpirationDate, p.Type })
                 .IsUnique(true);


        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if(!options.IsConfigured)
                options.UseSqlServer("Server=localhost;Initial Catalog=FinanceDb;Trusted_Connection=True;");
        }
    }
}