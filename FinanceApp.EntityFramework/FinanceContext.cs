using FinancialAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace FinancialApi.WebAPI.Data
{
    public class FinanceContext : DbContext
    {

        public FinanceContext(DbContextOptions<FinanceContext> options) : base(options)
        {

        }
        public FinanceContext()
        {

        }

        public DbSet<IndexValue> IndexValues { get; set; }
        //public DbSet<InvestmentFundValue> InvestmentFundValues { get; set; }
        //public DbSet<InvestmentFundValueHistoric> InvestmentFundValueHistoric { get; set; }
        //public DbSet<Asset> Assets { get; set; }
        public DbSet<TreasuryBondValue> TreasuryBondValues { get; set; }
        //public DbSet<TreasuryBondValueHistoric> TreasuryBondValueHistoric { get; set; }
        public DbSet<ProspectIndexValue> ProspectIndexValues { get; set; }
        //public DbSet<AssetChange> AssetChanges { get; set; }
        //public DbSet<AssetEarning> AssetEarnings { get; set; }
        public DbSet<Holiday> Holidays{ get; set; }
        public DbSet<WorkingDaysByYear> WorkingDaysByYear { get; set; }
        //public DbSet<IndexLastValue> IndexLastValues { get; set; }

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



            modelBuilder.Entity<TreasuryBondValue>()
                 .HasIndex(p => new { p.Date, p.ExpirationDate, p.Type })
                 .IsUnique(true);

        }


        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {

            options.UseSqlServer("Server=localhost;Initial Catalog=FinanceDb;Trusted_Connection=True;");
        }

    }
}