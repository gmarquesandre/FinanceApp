﻿// <auto-generated />
using System;
using FinanceApp.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FinanceApp.EntityFramework.Migrations
{
    [DbContext(typeof(FinanceContext))]
    partial class FinanceContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("FinanceApp.Shared.Models.CommonTables.CustomIdentityUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "2679b563-5a77-4731-b87a-d71d625f776a",
                            Email = "admin@admin.com",
                            EmailConfirmed = true,
                            LockoutEnabled = false,
                            NormalizedEmail = "ADMIN@ADMIN.COM",
                            NormalizedUserName = "ADMIN",
                            PasswordHash = "AQAAAAEAACcQAAAAECJ5J83Nm3kjpM58+nw7bPflBiutIFZyvrb8YiNxdB46VUq1BFwa0XjPIRpT8WY2dw==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "e33ba2f3-6d43-4ae8-adb1-96749e8cdc5b",
                            TwoFactorEnabled = false,
                            UserName = "admin"
                        });
                });

            modelBuilder.Entity("FinanceApp.Shared.Models.CommonTables.Holiday", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateLastUpdate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("Date")
                        .IsUnique();

                    b.ToTable("Holidays");
                });

            modelBuilder.Entity("FinanceApp.Shared.Models.CommonTables.IndexValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateEnd")
                        .HasColumnType("datetime2");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<double>("Value")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("Date", "DateEnd", "Index")
                        .IsUnique();

                    b.ToTable("IndexValues");
                });

            modelBuilder.Entity("FinanceApp.Shared.Models.CommonTables.ProspectIndexValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<double>("Average")
                        .HasColumnType("float");

                    b.Property<int>("BaseCalculo")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateEnd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateLastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateResearch")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateStart")
                        .HasColumnType("datetime2");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<double>("Max")
                        .HasColumnType("float");

                    b.Property<double>("Median")
                        .HasColumnType("float");

                    b.Property<double>("Min")
                        .HasColumnType("float");

                    b.Property<int>("ResearchAnswers")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DateStart", "DateEnd", "Index", "BaseCalculo")
                        .IsUnique();

                    b.ToTable("ProspectIndexValues");
                });

            modelBuilder.Entity("FinanceApp.Shared.Models.CommonTables.TreasuryBondTitle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("FixedInterestValueBuy")
                        .HasColumnType("float");

                    b.Property<double>("FixedInterestValueSell")
                        .HasColumnType("float");

                    b.Property<DateTime>("LastUpdateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<decimal>("UnitPriceBuy")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("UnitPriceSell")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("ExpirationDate", "Type")
                        .IsUnique();

                    b.ToTable("TreasuryBondTitles");
                });

            modelBuilder.Entity("FinanceApp.Shared.Models.CommonTables.TreasuryBondValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<double>("FixedInterestValueBuy")
                        .HasColumnType("float");

                    b.Property<double>("FixedInterestValueSell")
                        .HasColumnType("float");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<decimal>("UnitPriceBuy")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("UnitPriceSell")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("Date", "ExpirationDate", "Type")
                        .IsUnique();

                    b.ToTable("TreasuryBondValues");
                });

            modelBuilder.Entity("FinanceApp.Shared.Models.CommonTables.WorkingDaysByYear", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("DateLastUpdate")
                        .HasColumnType("datetime2");

                    b.Property<int>("WorkingDays")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Year")
                        .IsUnique();

                    b.ToTable("WorkingDaysByYear");
                });

            modelBuilder.Entity("FinanceApp.Shared.Models.UserTables.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("UpdateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("FinanceApp.Shared.Models.UserTables.CreditCard", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("InvoiceClosingDay")
                        .HasColumnType("int");

                    b.Property<int>("InvoicePaymentDay")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("CreditCards");
                });

            modelBuilder.Entity("FinanceApp.Shared.Models.UserTables.CurrentBalance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("CdIncome")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("CurrentValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("PercentageCdi")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("UpdateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("CurrentBalances");
                });

            modelBuilder.Entity("FinanceApp.Shared.Models.UserTables.FGTS", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<bool>("AnniversaryWithdraw")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("CurrentValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("MonthlyGrossIncome")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("UpdateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("FGTS");
                });

            modelBuilder.Entity("FinanceApp.Shared.Models.UserTables.Income", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("InitialDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsEndless")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Recurrence")
                        .HasColumnType("int");

                    b.Property<int?>("TimesRecurrence")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Incomes");
                });

            modelBuilder.Entity("FinanceApp.Shared.Models.UserTables.Loan", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("InitialDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("InterestRate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("LoanValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("MonthsPayment")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Loans");
                });

            modelBuilder.Entity("FinanceApp.Shared.Models.UserTables.PrivateFixedIncome", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("AdditionalFixedInterest")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<decimal>("IndexPercentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("InvestmentDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("LiquidityOnExpiration")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PreFixedInvestment")
                        .HasColumnType("bit");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("PrivateFixedIncomes");
                });

            modelBuilder.Entity("FinanceApp.Shared.Models.UserTables.Spending", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int?>("CreditCardId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("InitialDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsEndless")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRequired")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Payment")
                        .HasColumnType("int");

                    b.Property<int>("Recurrence")
                        .HasColumnType("int");

                    b.Property<int?>("TimesRecurrence")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("CreditCardId");

                    b.HasIndex("UserId");

                    b.ToTable("Spendings");
                });

            modelBuilder.Entity("FinanceApp.Shared.Models.UserTables.TreasuryBond", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("ExpirationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("InvestmentDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Operation")
                        .HasColumnType("int");

                    b.Property<decimal>("Quantity")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("UpdateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("TreasuryBonds");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("FinanceApp.Shared.Models.UserTables.Category", b =>
                {
                    b.HasOne("FinanceApp.Shared.Models.CommonTables.CustomIdentityUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FinanceApp.Shared.Models.UserTables.CreditCard", b =>
                {
                    b.HasOne("FinanceApp.Shared.Models.CommonTables.CustomIdentityUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FinanceApp.Shared.Models.UserTables.CurrentBalance", b =>
                {
                    b.HasOne("FinanceApp.Shared.Models.CommonTables.CustomIdentityUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FinanceApp.Shared.Models.UserTables.FGTS", b =>
                {
                    b.HasOne("FinanceApp.Shared.Models.CommonTables.CustomIdentityUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FinanceApp.Shared.Models.UserTables.Income", b =>
                {
                    b.HasOne("FinanceApp.Shared.Models.CommonTables.CustomIdentityUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FinanceApp.Shared.Models.UserTables.Loan", b =>
                {
                    b.HasOne("FinanceApp.Shared.Models.CommonTables.CustomIdentityUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FinanceApp.Shared.Models.UserTables.PrivateFixedIncome", b =>
                {
                    b.HasOne("FinanceApp.Shared.Models.CommonTables.CustomIdentityUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FinanceApp.Shared.Models.UserTables.Spending", b =>
                {
                    b.HasOne("FinanceApp.Shared.Models.UserTables.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.HasOne("FinanceApp.Shared.Models.UserTables.CreditCard", "CreditCard")
                        .WithMany()
                        .HasForeignKey("CreditCardId");

                    b.HasOne("FinanceApp.Shared.Models.CommonTables.CustomIdentityUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("CreditCard");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FinanceApp.Shared.Models.UserTables.TreasuryBond", b =>
                {
                    b.HasOne("FinanceApp.Shared.Models.CommonTables.CustomIdentityUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("FinanceApp.Shared.Models.CommonTables.CustomIdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("FinanceApp.Shared.Models.CommonTables.CustomIdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole<int>", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FinanceApp.Shared.Models.CommonTables.CustomIdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("FinanceApp.Shared.Models.CommonTables.CustomIdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
