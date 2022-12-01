using Microsoft.EntityFrameworkCore;

namespace LinkIt.Models.Entities
{
    public class LinkItContext : DbContext
    {
        public DbSet<User>? Users { get; set; }
        public DbSet<Link>? Links { get; set; }
        public DbSet<Vote>? Votes { get; set; }
        public DbSet<Comment>? Comments { get; set; }
        public LinkItContext()
        {

        }

        public LinkItContext(DbContextOptions<LinkItContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Vote>().HasKey(v => new { v.LinkId, v.UserId });
        }
    }
}
