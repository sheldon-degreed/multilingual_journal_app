using Microsoft.EntityFrameworkCore;
using MultilingualJournalApp.Models;

namespace MultilingualJournalApp.Data
{
    public class JournalContext : DbContext
    {
        public JournalContext(DbContextOptions<JournalContext> options) : base(options)
        {
        }

        public DbSet<JournalEntry> JournalEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JournalEntry>()
                .Property(e => e.CreatedDate)
                .HasDefaultValueSql("datetime('now')");

            base.OnModelCreating(modelBuilder);
        }
    }
}