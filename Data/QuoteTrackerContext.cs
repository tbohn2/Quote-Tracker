using Microsoft.EntityFrameworkCore;
using Quote_Tracker.Models;
using System.Reflection;

namespace Quote_Tracker.Data
{
    public class Quote_Tracker_Context : DbContext
    {
        public Quote_Tracker_Context(DbContextOptions<Quote_Tracker_Context> options) : base(options) { }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Book> Books { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<QuoteTopic> QuoteTopics { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Quote>()
                .Property(q => q.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            modelBuilder.Entity<Quote>()
                .HasOne(q => q.Book)
                .WithMany(b => b.Quotes)
                .HasForeignKey(q => q.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuoteTopic>()
                .HasKey(qt => new { qt.QuoteId, qt.TopicId });

            modelBuilder.Entity<QuoteTopic>()
                .HasOne(qt => qt.Quote)
                .WithMany(q => q.QuoteTopics)
                .HasForeignKey(qt => qt.QuoteId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<QuoteTopic>()
                .HasOne(qt => qt.Topic)
                .WithMany(t => t.QuoteTopics)
                .HasForeignKey(qt => qt.TopicId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}