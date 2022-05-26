using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UsuariosApi.Models;

namespace FinanceApp.EntityFramework.Auth
{
    public class UserDbContext : IdentityDbContext<CustomIdentityUser, IdentityRole<int>, int>
    {


        public UserDbContext(DbContextOptions<UserDbContext> opt) : base(opt)
        {
        }
        public UserDbContext()
        {
        }

        public DbSet<Teste> Testes { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {

            options.UseSqlServer("Server=localhost;Initial Catalog=AuthDb;Trusted_Connection=True;");
        }

    }
}