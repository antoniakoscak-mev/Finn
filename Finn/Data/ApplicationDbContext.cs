using Finn.Models;
using Microsoft.EntityFrameworkCore;

namespace Finn.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Trosak> Troskovi { get; set; }

        public DbSet<Prihod> Prihodi { get; set; }
    }
}