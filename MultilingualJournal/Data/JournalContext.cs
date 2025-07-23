using Microsoft.EntityFrameworkCore;
using MultilingualJournal.Models;

namespace MultilingualJournal.Data
{
    public class JournalContext : DbContext
    {
        public JournalContext(DbContextOptions<JournalContext> options) : base(options)
        {
        }

        public DbSet<JournalEntry> JournalEntries { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Translation> Translations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<JournalEntry>()
                .HasMany(j => j.Tags)
                .WithMany(t => t.JournalEntries)
                .UsingEntity(j => j.ToTable("JournalEntryTags"));

            modelBuilder.Entity<Translation>()
                .HasOne(t => t.JournalEntry)
                .WithMany(j => j.Translations)
                .HasForeignKey(t => t.JournalEntryId);

            modelBuilder.Entity<Tag>()
                .HasIndex(t => t.Name)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}