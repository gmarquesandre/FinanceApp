using FinanceApp.Shared.Models;
using FinancialAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace FinancialApi.WebAPI.Data
{
    public class FinanceContext : DbContext
    {

        public FinanceContext(DbContextOptions<FinanceContext> options) : base(options)
        {

        }       

        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {                       
            modelBuilder.Entity<IndexValue>()
               .HasIndex(p => new { p.Date, p.DateEnd, p.Index})
               .IsUnique(true);

            modelBuilder.Entity<ProspectIndexValue>()
               .HasIndex(p => new { p.DateStart, p.DateEnd, p.Index, p.BaseCalculo })
               .IsUnique(true);


            modelBuilder.Entity<WorkingDaysByYear>()
                 .HasIndex(p => new { p.Year })
                 .IsUnique(true);


            modelBuilder.Entity<Holiday>()
                 .HasIndex(p => new { p.Date })
                 .IsUnique(true);

            modelBuilder.Entity<TreasuryBondTitle>()
                .HasIndex(p => new {p.ExpirationDate, p.Type})
                .IsUnique(true);

            modelBuilder.Entity<TreasuryBondValue>()
                 .HasIndex(p => new { p.Date, p.ExpirationDate, p.Type })
                 .IsUnique(true);

        }        

    }
}