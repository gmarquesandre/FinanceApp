using FinancialAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace FinancialApi.WebAPI.Data
{
    public class FinanceContext : DbContext
    {

        public FinanceContext(DbContextOptions<FinanceContext> options) : base(options)
        {

        }

        public DbSet<IndexValue> IndexValues { get; set; }
        public DbSet<InvestmentFundValue> InvestmentFundValues { get; set; }
        public DbSet<InvestmentFundValueHistoric> InvestmentFundValueHistoric { get; set; }
        public DbSet<Asset> Assets { get; set; }
        public DbSet<TreasuryBondValue> TreasuryBondValues { get; set; }
        public DbSet<TreasuryBondValueHistoric> TreasuryBondValueHistoric { get; set; }
        public DbSet<ProspectIndexValue> ProspectIndexValues { get; set; }
        public DbSet<AssetChange> AssetChanges { get; set; }
        public DbSet<AssetEarning> AssetEarnings { get; set; }
        public DbSet<Holiday> Holidays{ get; set; }
        public DbSet<WorkingDaysByYear> WorkingDaysByYear { get; set; }
        public DbSet<IndexLastValue> IndexLastValues { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asset>()
                .HasIndex(p => new { p.AssetCodeISIN })
                .IsUnique(true);


            modelBuilder.Entity<AssetChange>()
               .HasIndex(p => new { p.Hash})
               .IsUnique(true);

            modelBuilder.Entity<InvestmentFundValueHistoric>()
               .HasIndex(p => new { p.Date, p.CNPJ })
               .IsUnique(true);


            modelBuilder.Entity<AssetEarning>()
               .HasIndex(p => new { p.Hash })
               .IsUnique(true);

            modelBuilder.Entity<Holiday>()
             .HasIndex(p => new { p.CountryCode, p.Date})
             .IsUnique(true);


            modelBuilder.Entity<IndexValue>()
               .HasIndex(p => new { p.Date, p.IndexName})
               .IsUnique(true);


            modelBuilder.Entity<WorkingDaysByYear>()
                 .HasIndex(p => new { p.Year })
                 .IsUnique(true);

            modelBuilder.Entity<IndexLastValue>()
                .HasIndex(p => new { p.IndexName })
                .IsUnique(true);

            modelBuilder.Entity<TreasuryBondValueHistoric>()
                .HasIndex(p => new { p.CodeISIN, p.Date })
                .IsUnique(true);



        }     

    }
}