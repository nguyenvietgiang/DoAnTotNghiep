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
        public DbSet<JobPosting> JobPostings { get; set; }
        public DbSet<JobApplyForm> JobApplyForms { get; set; }
        public DbSet<RevenueStatistic> RevenueStatistics { get; set; }
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<AnswerCount> AnswerCounts { get; set; }
        public DbSet<ImageGalery> ImageGaleries { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Policy> Policies { get; set; }
        public DbSet<Pay> Pays { get; set; }
        public DbSet<OnlineResume> OnlineResumes { get; set;}
        public DbSet<Message> Messages { get; set; }
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Event>()
                .Property(e => e.RowVersion)
                .IsRowVersion(); // Cấu hình RowVersion cho việc theo dõi xung đột
        }
    }
}
