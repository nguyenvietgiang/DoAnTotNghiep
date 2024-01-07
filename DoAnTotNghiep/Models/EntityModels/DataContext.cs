using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace DoAnTotNghiep.Models.EntityModels
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Candidate> Candidates { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<CvLibrary> CvLibraries { get; set; }
        public DbSet<Discuss> Discusses { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<JobPosting> JobPostings { get; set; }
        public DbSet<JobApplyForm> JobApplyForms { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Message>()
        .HasOne(m => m.Sender)
        .WithMany(a => a.SentMessages)
        .HasForeignKey(m => m.SenderID)
        .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Receiver)
                .WithMany(a => a.ReceivedMessages)
                .HasForeignKey(m => m.ReceiverID)
                .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
