using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HLFundView.Models;

namespace HLFundView.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Fund> Fund { get; set; }

        public DbSet<Dividend> Dividends { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Dividend>()
                  .HasKey(m => new { m.Symbol, m.DividendExDate, m.DividendAmount });
        }
    }

}