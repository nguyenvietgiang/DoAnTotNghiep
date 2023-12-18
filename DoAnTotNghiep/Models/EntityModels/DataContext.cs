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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

    }
}
     