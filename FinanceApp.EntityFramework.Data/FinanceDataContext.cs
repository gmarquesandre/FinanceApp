using FinanceApp.Shared.Entities.CommonTables;
using Microsoft.EntityFrameworkCore;

namespace FinanceApp.EntityFramework.Data
{
    public class FinanceDataContext : DbContext
    {
        public FinanceDataContext(DbContextOptions<FinanceDataContext> opt) : base(opt)
        {

        }

        public DbSet<IndexValue> IndexValues => Set<IndexValue>();
        public DbSet<TreasuryBondValue> TreasuryBondValues => Set<TreasuryBondValue>();
        public DbSet<ProspectIndexValue> ProspectIndexValues => Set<ProspectIndexValue>();
        public DbSet<Holiday> Holidays => Set<Holiday>();
        public DbSet<WorkingDaysByYear> WorkingDaysByYear => Set<WorkingDaysByYear>();
        public DbSet<Asset> Assets => Set<Asset>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

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

    }
}