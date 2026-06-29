using Finn.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Finn.Data
{
    public class ApplicationDbContext
        : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Trosak> Troskovi { get; set; }

        public DbSet<Prihod> Prihodi { get; set; }

        public DbSet<Budzet> Budzeti { get; set; }

        public DbSet<KategorijaPrihoda> KategorijePrihoda { get; set; }

        public DbSet<KategorijaRashoda> KategorijeRashoda { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<UserSettings> UserSettings { get; set; }
    }
}